using UnityEngine;
using System;

public class EffectData
{
    private EffectType type;
    private int value;

    public int Value { get { return value; } }
    public EffectType Type { get { return type; } }

    public EffectData() { }
    public EffectData(EffectType tp, int val)
    {
        type = tp;
        value = val;
    }
}

public static class EffectSignals
{
    // Событие вызывается, когда на колобка накладывается визуальный статус
    public static event Action<EffectData> OnStatusApplied;

    public static event Action<int> OnEnemyLoss;
    public static event Action<int> OnDecrimentInventoryItem;

    public static void RaiseStatusApplied(EffectData data)
    {
        OnStatusApplied?.Invoke(data);
    }

    public static void RaiseEnemyLoss(int value)
    {
        OnEnemyLoss?.Invoke(value);
    }

    public static void RaiseDecrimentInventoryItem(int value)
    {
        OnDecrimentInventoryItem?.Invoke(value);
    }
}
