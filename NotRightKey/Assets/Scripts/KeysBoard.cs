using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KeysBoard : MonoBehaviour
{
    private List<Vector3> AllKeyPositions = new List<Vector3>();

    [Header("Settings")]
    [Tooltip("На сколько единиц улетать за пределы карты")]
    [SerializeField] private float outOfBoundsOffset = 2f;    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Material[] materials;
    [SerializeField] private GameObject[] bonusPrefabs;
    //[SerializeField] private ... [] effects;

    private LevelInfo _levelInfo = null;

    void Start()
    {
        CacheAllKeyPositions();
    }

    public void SetLevelInfo(LevelInfo info)
    {
        _levelInfo = info;

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

    public void SetParamToKeys()
    {

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
}
