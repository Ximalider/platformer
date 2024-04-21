using UnityEngine;
using UnityEngine.Events;
using System;

public enum SwitchType
{
   Loop,
   PingPong,
   Once
}

public class SwitchTrigger : Trigger
{
    
    [SerializeField] private SwitchType type;
    [SerializeField, Min(0)] private int state;
    [SerializeField] private UnityEvent[] events;

    private int State
    {
        get
            {
                switch(type)
                {
                case SwitchType.Loop:
                    return state % events.Length;
                case SwitchType.PingPong:
                    return (int)Mathf.PingPong(state, events.Length - 1);
                case SwitchType.Once:
                    return state;
                default:
                throw new ArgumentOutOfRangeException();
            }
        }
    }

    public override void Activate(Collider2D other)
    {
        if (type == SwitchType.Once && State >= events.Length) return;
        
        events[State]?.Invoke();
        state++;
    }
}

