using System.Linq;
using UnityEngine;

public enum ActivationType
{
    Enter,
    Exit
}

public abstract class Trigger : MonoBehaviour
{
    [Header("Trigger")]
    [SerializeField] private ActivationType activation;
    [SerializeField] private string[] tags = { "Player" };
    [SerializeField] private bool once;

    private bool _done;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!tags.Contains(collision.tag)) return;
        if (activation != ActivationType.Enter || _done) return;

        _done = once;
        Activate(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!tags.Contains(collision.tag)) return;
        if (activation != ActivationType.Exit || _done) return;

        _done = once;
        Activate(collision);
    }

    public abstract void Activate(Collider2D other);
}
