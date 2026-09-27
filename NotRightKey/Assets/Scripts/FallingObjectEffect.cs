using UnityEngine;

public class FallingObjectEffect : KeyEffectBase
{
    [Header("Falling Settings")]
    //[SerializeField] private GameObject fallingPrefab; // Камень или молния
    [SerializeField] private float fallHeight = 10f;
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private EffectType type = EffectType.None;

    private GameObject fallingPrefab; // Камень или молния
    private GameObject fallingObject;

    public override EffectType Type { get { return type; } }
    protected override void PlayVisual(Transform heroPos)
    {
        Vector3 spawnPos = heroPos.position + Vector3.up * fallHeight;
        fallingObject = Instantiate(fallingPrefab, spawnPos, Quaternion.identity); // Можно добавить Rigidbody для физики падения здесь
    }
    protected override void ApplyLogic(Transform heroPos)
    {
        // Логика нанесения урона будет вызвана ЗДЕСЬ после того, как сработает защита
        Debug.Log($"Применяется эффект {Type} к игроку");
        // HealthSystem.Instance.TakeDamage(damageAmount);
    }

    public void SetParams(GameObject prefab, int damage, EffectType effectType)
    {
        fallingPrefab = prefab;
        damageAmount = damage;
        type = effectType;
    }
}
