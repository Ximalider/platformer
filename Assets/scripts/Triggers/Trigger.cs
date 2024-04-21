using System.Linq;
using UnityEngine;

public enum ActivationType
{
    Enter,
    Exit,
    Interactive
}
public abstract class Trigger : MonoBehaviour, Interactive
{
    [Header("Trigger")]
    [SerializeField] private ActivationType activation;
    [SerializeField] private int priority;
    [SerializeField] private string[] tags = { "Player" };
    [SerializeField] private bool once;

    private bool _done;

    public int Priority => priority;
 

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

    public void Interact(Interactor interactor)
    {
        if (!CanInteractWith(interactor)) return;
        _done = once;
        Activate(interactor.GetComponent<Collider2D>());
    }

    public bool CanInteractWith(Interactor interactor)
    {
        return tags.Contains(interactor.tag)
            && activation == ActivationType.Interactive
            && !_done;
    }
}
