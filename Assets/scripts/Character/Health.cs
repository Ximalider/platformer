using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public float damage;
}

public interface DamageAble
{
    public float TakeDamage(DamageInfo damageInfo);
}

public interface Health
{
    public float Max { get; }
    public float Ratio { get; }
    bool IsAlive { get; }

    public event Action<Health, DamageInfo> onDamage;
    public event Action<Health, DamageInfo> onDeath;

    bool CanBeDamaged(DamageInfo damageInfo);
    float TakeDamage(DamageInfo damageInfo);
}

public class Health : MonoBehaviour, Health
{
    [SerializeField] private float max;
    [SerializeField] private float current;

    private float Current
    {
        get => current;
        set => current = Mathf.Clamp(value, 0f, max);
    }

    public float Max => max;

    public float Ratio => current / max;

    public bool IsAlive => current > 0f;

    public event Action<Health, DamageInfo> onDamage;
    public event Action<Health, DamageInfo> onDeath;
    private void OnValidate()
    {
        Current = Current;
    }
    public bool CanBeDamaged(DamageInfo damageInfo)
    {
        return IsAlive;
    }
    public float TakeDamage(DamageInfo damageInfo)
    {
        if (damageInfo.damage < 0f) return 0f;
        if (!CanBeDamaged(damageInfo)) return 0f;
        var oldCurrent = Current;
        onDamage?.Invoke(this, damageInfo);
        if (!IsAlive) onDeath?.Invoke(this, damageInfo);
        return Mathf.Abc(oldCurrent - Current);
    }
}
