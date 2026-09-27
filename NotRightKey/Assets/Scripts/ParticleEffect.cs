using UnityEngine;

public class ParticleEffect : KeyEffectBase
{
    private ParticleSystem effectPS;
    private float height = 1f;
    private float delay = 1f;
    private int valueEffect;
    private EffectType effectType;

    private Vector3 oldPos;
    public override EffectType Type { get { return effectType; } }

    protected override void ApplyLogic(Transform heroPosition)
    {
        // Логика нанесения урона будет вызвана ЗДЕСЬ после того, как сработает защита
        Debug.Log($"Применяется эффект {Type} к игроку");
    }

    protected override void PlayVisual(Transform heroPosition)
    {
        Vector3 pos = heroPosition.position;
        oldPos = effectPS.transform.position;
        pos.y += height;
        effectPS.transform.position = pos;
        effectPS.gameObject.SetActive(true);
        effectPS.Play();
        EffectSignals.RaiseStatusApplied(new EffectData(effectType, valueEffect));
        Invoke("EndEffect", delay);
   }

    private void EndEffect()
    {
        effectPS.Stop();
        effectPS.gameObject.SetActive(false);
        effectPS.transform.position = oldPos;
    }

    public void SetParams(ParticleSystem ps, EffectType tp, float h, float d, int value)
    {
        effectPS = ps;
        effectType = tp;
        height = h;
        delay = d;
        valueEffect = value;
    }
}
