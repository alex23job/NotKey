using UnityEngine;

public interface IKeyEffect
{
    /// <summary>
    /// Тип эффекта нужен для проверки защит в инвентаре
    /// </summary>
    EffectType Type { get; }
    
    /// <summary>
    /// Основная логика: что происходит при приземлении на клавишу
    /// </summary>
    void Execute(Transform heroPosition);
}

/// <summary>
/// Перечисление типов для удобной фильтрации щитов
/// </summary>
public enum EffectType 
{
    None,
    FallingRock,        // Падающий камень
    Lightning,          // Молния сверху
    FireParticles,      // Огонь под ногами
    PoisonParticles,    // Ядовитое облако
    HealParticles,      // Лечение
    WaterSplash,        // Вода
    Oil,                // Масло под ногами
    RandomMoving        // Случайное перещение на соседнюю клавишу
}
