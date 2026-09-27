using UnityEngine;

public abstract class KeyEffectBase : MonoBehaviour, IKeyEffect
{
    public abstract EffectType Type { get; }
    [SerializeField] protected float effectDuration = 1f; // Длительность жизни объекта/системы
                                                          
    public virtual void Execute(Transform heroPosition)
    {
        PlayVisual(heroPosition);
        ApplyLogic(heroPosition); // Нанесение урона, лечение и т.д.
    }
    
    protected abstract void PlayVisual(Transform heroPosition);
    protected abstract void ApplyLogic(Transform heroPosition);
    
    // Вспомогательный метод для уничтожения через время
    protected void DestroyWithDelay(GameObject obj, float delay)
    {
        if (obj != null) Destroy(obj, delay);
    }
}
