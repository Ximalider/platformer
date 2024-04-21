using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class CollectableItem: MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField, Min(0)] private int amount = 1;
    [Space]
    [SerializeField] private UnityEvent onPikeUp;

    private void OnValidate()
    {
        if (!item) return;

        GetComponent<SpriteRenderer>().sprite = item.Sprite;
        amount = Math.Clamp(amount, 0, item.MaxAmount);
    }
    public void PickUp(Inventory inventory)
    {
        if (!inventory) return;
        amount -= inventory.Pickup(item, amount);

        if (amount <= 0)
        {
            onPikeUp?.Invoke();
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp(collision.GetComponent<Inventory>());
    }
}