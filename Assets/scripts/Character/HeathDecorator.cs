using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HealthDecorator: MonoBehaviour, IHealth
{
    private IHealth _health;

    public virtual float Max => _health.Max;

    public virtual float Ratio => _health.Ratio;

    public virtual bool IsAlive => _health.IsAlive;

    public virtual event Action<IHealth, DamageInfo> onDamage
    {
        add
        {
            _health.onDamage += value;
        }

        remove
        {
            _health.onDamage -= value;
        }
    }

    public virtual event Action<IHealth, DamageInfo> onDeath
    {
        add
        {
            _health.onDeath += value;
        }

        remove
        {
            _health.onDeath -= value;
        }
    }

    public bool CanBeDamaged(DamageInfo damageInfo)
    {
        return _health.CanBeDamaged(damageInfo);
    }

    public float TakeDamage(DamageInfo damageInfo)
    {
        return _health.TakeDamage(damageInfo);
    }

    public virtual IHealth Assign(IHealth health)
    {
        _health = health;
        return this;
    }
}

public class DamageCooldownDecorator : HealthDecorator
{
    [SerializeField] private float cooldownTime = 1f;

    private bool _isInCooldown;
    
    public virtual bool CanBeDamaged(DamageInfo damageInfo)
    {
        return !_isInCooldown && base.CanBeDamaged(damageInfo);
    }

    public virtual float TakeDamage(DamageInfo damageInfo)
    {
        return _isInCooldown ? 0f : base.TakeDamage(damageInfo);
    }

    public override IHealth Assign(IHealth health)
    {
        health.onDamage += OnDamage;
        return base.Assign(health);
    }
    
    private void OnDamage(IHealth comp, DamageInfo damageInfo)
    {
        CoroutineRunner.instance.StartCoroutine(DoCooldown());
    }
    private IEnumerator DoCooldown()
    {
        _isInCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        _isInCooldown = false;
    }
}
