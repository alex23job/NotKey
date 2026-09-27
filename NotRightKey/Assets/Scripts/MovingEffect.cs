using UnityEngine;

public class MovingEffect : KeyEffectBase
{
    public override EffectType Type { get { return effectType; } }
    private GameObject prefabGO = null, effectObject = null;
    private float height = 1f;
    private float delay = 1f;
    private EffectType effectType;

    protected override void ApplyLogic(Transform heroPosition)
    {
        Debug.Log($"Применяется эффект {Type} к игроку");
    }

    protected override void PlayVisual(Transform heroPos)
    {
        Vector3 spawnPos = heroPos.position + Vector3.up * height;
        effectObject = Instantiate(prefabGO, spawnPos, Quaternion.Euler(new Vector3(-90f, 0, -90f))); // Можно добавить Rigidbody для физики падения здесь
        Destroy(effectObject, delay);
    }
    public void SetParams(GameObject prefab, EffectType tp, float h, float d)
    {
        prefabGO = prefab;
        effectType = tp;
        height = h;
        delay = d;
    }
}
