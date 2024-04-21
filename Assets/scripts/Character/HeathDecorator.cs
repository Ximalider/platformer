using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDecorator: MonoBehaviour
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

    public virtual bool CanBeDamaged(DamageInfo damageInfo)
    {
        return !isInCooldown && base.CanBeDamaged(damageInfo);
    }

    public virtual float TakeDamage(DamageInfo damageInfo)
    {
        return _isInCooldown ? 0f : base.TakeDamage(damageInfo);
    }

    public override HealthDecorator Assign(Health health)
    {
        health.onDamage += OnDamage;
    }
    private void OnDamage(Health comp, DamageInfo damageInfo)
    {
        CoroutineRunner.instanse.StartCoroutine(DoCooldown());
    }
    private IEnumerator DoCooldown()
    {
        _isInCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        _isInCooldown = false;
    }
}
