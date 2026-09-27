using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class KeysBoard : MonoBehaviour
{
    private List<Vector3> AllKeyPositions = new List<Vector3>();

    [Header("Settings")]
    [Tooltip("На сколько единиц улетать за пределы карты")]
    [SerializeField] private float outOfBoundsOffset = 2f;    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Material[] materials;
    [SerializeField] private GameObject[] bonusPrefabs;
    [SerializeField] private GameObject[] fallingPrefabs;
    [SerializeField] private ParticleSystem[] particleSystems;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject finishPrefab;

    private LevelInfo _levelInfo = null;

    private ParticleSystem[] _particles;

    public LevelInfo CurrentLevel { get { return _levelInfo; } }

    void Start()
    {
        CacheAllKeyPositions();
    }

    public void SetLevelInfo(LevelInfo info)
    {
        _levelInfo = info;
        SetParamToKeys();

    }

    /// <summary> 
    /// Собирает Transform всех дочерних объектов-клавиш в единый массив координат.
    /// </summary>
    private void CacheAllKeyPositions()
    {
        AllKeyPositions.Clear(); // Ищем во всех вложенных Line_1...Line_5
        foreach (Transform line in transform)
        {
            foreach (Transform key in line)
            { // Сохраняем позицию центра клавиши
                AllKeyPositions.Add(key.position);
            }
        }
        Debug.Log($"[KeysBoard] Cached {AllKeyPositions.Count} keys.");
    }

    public void ViewColorEffects(bool value)
    {
        foreach (Transform line in transform)
        {
            foreach (Transform key in line)
            {
                KeyControl keyControl = key.GetComponent<KeyControl>();
                if (keyControl != null)
                {
                    keyControl.ViewEffectMat(value);
                }
            }
        }
    }

    public void SetParamToKeys()
    {
        int numEffect = 0;
        int numBonus = -1;
        GameObject bonus = null;
        IKeyEffect keyEffect = null;
        int[] ps_value = { 20, 15, 20, 0 };
        float[] ps_heigts = { 1f, 1f, 2f, 3f};
        EffectType[] effectTypes = { EffectType.FireParticles, EffectType.PoisonParticles, EffectType.HealParticles, EffectType.WaterSplash, EffectType.Oil, EffectType.RandomMoving };

        _particles = new ParticleSystem[particleSystems.Length];
        for (int j = 0; j < particleSystems.Length; j++)
        {
            _particles[j] = Instantiate(particleSystems[j], new Vector3(0, -3f, -10f), Quaternion.Euler(new Vector3(-90f, 0, 0)));
            if (j == 2) _particles[j].transform.rotation = Quaternion.Euler(Vector3.zero);
            if (j == 3) _particles[j].transform.rotation = Quaternion.Euler(new Vector3(90f, 0, 0));
            _particles[j].gameObject.SetActive(false);
        }

        foreach (Transform line in transform)
        {
            foreach (Transform key in line)
            {
                KeyControl keyControl = key.GetComponent<KeyControl>();
                if (keyControl != null)
                {
                    if (keyControl.KeyID == _levelInfo.FinishKey)
                    {
                        GameObject fin = Instantiate(finishPrefab, key.position, Quaternion.identity);
                        fin.transform.parent = key;
                        fin.transform.localPosition = new Vector3(0, 1f, 0);
                    }
                    if  (_levelInfo.EffectArr.Contains(keyControl.KeyID))
                    {
                        numEffect = Random.Range(1, 9);
                        //print($"id = {keyControl.KeyID}    numEffect = {numEffect}");
                        if (numEffect < 3)
                        {   //  falling
                            key.AddComponent<FallingObjectEffect>();
                            FallingObjectEffect foef = key.GetComponent<FallingObjectEffect>();
                            foef.SetParams(fallingPrefabs[numEffect - 1], 10 * numEffect, (numEffect == 1) ? EffectType.FallingRock : EffectType.Lightning);
                        }
                        else if (numEffect >= 3 && numEffect < 7)
                        {   //  particle
                            key.AddComponent<ParticleEffect>();
                            ParticleEffect pef = key.GetComponent<ParticleEffect>();
                            pef.SetParams(_particles[numEffect - 3], effectTypes[numEffect - 3], ps_heigts[numEffect - 3], 2f, ps_value[numEffect - 3]);
                        }
                        else
                        {   //  moving
                            key.AddComponent<MovingEffect>();
                            MovingEffect mof = key.GetComponent<MovingEffect>();
                            mof.SetParams(fallingPrefabs[numEffect - 5], effectTypes[numEffect - 3], 1f, 1f);
                        }
                        keyControl.SetParams(materials[numEffect - 1]);
                    }
                    if (_levelInfo.BonusArr.Contains(keyControl.KeyID))
                    {
                        numBonus = Random.Range(0, bonusPrefabs.Length);
                        bonus = bonusPrefabs[numBonus];
                        keyControl.SetBonus(bonus);
                    }
                    if (_levelInfo.MonstrArr.Contains(keyControl.KeyID))
                    {
                        Vector3 pos = key.transform.position;
                        pos.y += 2f;
                        GameObject enemy = Instantiate(enemyPrefabs[0], pos, Quaternion.identity);
                        EnemyControl enemyControl = enemy.GetComponent<EnemyControl>();
                        //enemyControl.SetParams(_particles[0], 50, 10, 1);
                        enemyControl.SetParams(50, 10, 1, keyControl.KeyID);
                        keyControl.IsEnemy = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Возвращает целевую позицию для колобка.
    /// Если впереди есть клавиша - возвращает её центр.
    /// Если впереди пустота - возвращает точку "в лаве".
    /// </summary>
    public Vector3 GetTargetPosition(Vector3 currentPos, Vector3 direction)
    {
        if (direction == Vector3.zero) return currentPos; // Находим ближайшую клавишу ВПЕРЕДИ героя

        Vector3 closestKey = FindClosestKeyInDirection(currentPos, direction); // Проверяем, нашли ли мы что-то вообще и находится ли оно дальше текущего положения

        bool isValidMove = closestKey != Vector3.zero && Vector3.Distance(currentPos, closestKey) > 0.1f;
        if (isValidMove)
        {
            return closestKey;
        }
        else
        { // КЛАВИШИ НЕТ -> УЛЕТАЕМ ЗА ПРЕДЕЛЫ КАРТЫ
            return currentPos + (direction * outOfBoundsOffset);
        }

    }

    /// <summary>
    /// Математический поиск ближайшей точки из массива в заданном секторе направления.
    /// </summary>
    private Vector3 FindClosestKeyInDirection(Vector3 fromPos, Vector3 dir)
    {
        const float maxDistance = 14f; // Дальность поиска (увеличьте, если доска огромная) 

        // Фильтруем только те клавиши, которые находятся спереди от нас
        var candidates = AllKeyPositions.Where(pos =>
        {
            Vector3 toCandidate = pos - fromPos; 

            if (dir == Vector3.left || dir == Vector3.right)
            {
                if (toCandidate.z > 0.1f) return false;
            }
            
            // Проверяем угол: скалярное произведение должно быть > 0 (значит, точка перед нами)
            if (Vector3.Dot(toCandidate.normalized, dir.normalized) < 0.7f) return false;
            
            // Проверяем дистанцию
            return toCandidate.magnitude < maxDistance;
        }).ToList();

        if (candidates.Count == 0) return Vector3.zero; // Ничего не найдено

        // Сортируем кандидатов по расстоянию и берем ближайший
        Vector3 res = candidates.OrderBy(pos => Vector3.Distance(fromPos, pos)).FirstOrDefault();
        GameObject key = FindKeyByPos(res);
        if (key != null)
        {
            KeyControl keyControl = key.GetComponent<KeyControl>();
            if (keyControl != null)
            {
                keyControl.PlayPush();
                keyControl.AcceptEffect();
            }
        }
        return res;       
    }

    public GameObject FindKeyByPos(Vector3 pos)
    {
        foreach (Transform line in transform)
        {
            foreach (Transform key in line)
            { // Сохраняем позицию центра клавиши
                if (Vector3.Distance(pos, key.position) < 0.1f)
                {
                    //print($"Цель => {key.name}");
                    return key.gameObject;
                }
            }
        }

        return null;
    }

    public void AcceptKeyEffect(Vector3 pos)
    {
        GameObject key = FindKeyByPos(pos);
        if (key != null)
        {
            KeyControl keyControl = key.GetComponent<KeyControl>();
            if (keyControl != null)
            {
                keyControl.AcceptEffect();
            }
        }
    }
}
