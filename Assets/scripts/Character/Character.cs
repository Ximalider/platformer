using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [Serializrable]
    private class HealthDecorators
    {
        [SerializeField] public FlatDamageDecorator flatDamage;
        [SerializeField] public DamageCooldownDecorator damageCooldown;
    }
    [SerializeField] private Health health;
    [Space]
    [SerializeField] private CharacterAnimator animator;
    [Space]
    [SerializeField] private HealthDecorators decorators;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var rot = transform.eulerAngles;

        rot.y = _rigidbody.velocity.x switch
        {
            > 0 => 0f,
            < 0 => 180f,
            _ => rot.y
        };

        transform.eulerAngles = rot;
    }
    private void Death(Health component, DamageInfo damageInfo)
    {
        //+анимация -управление
    }
    private void Death(Health component, DamageInfo damageInfo)
    {
        animator.Hurt();
    }
    public float TakeDamage(DamageInfo damageInfo)
    {
        return _health.TakeDamage(damageInfo);
    }
    private void Start()
    {
        _health = health;
        DecorateHealth(decorators.flatDamage);
        DecorateHealth(decorators.DamageCooldown);
    }

    public void DecorateHealth(HealthDecorator decorator)
    {
        _health = decorator.Asign(_health) ?? _health;
    }
}