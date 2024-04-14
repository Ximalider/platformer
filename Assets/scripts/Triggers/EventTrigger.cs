using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : Trigger
{
    [SerializeField] private UnityEvent OnEvent;

    public override void Activate(Collider2D other)
    {
        OnEvent?.Invoke();
    }
}
