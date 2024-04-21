using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private Sprite sprite;
    [SerializeField, Min(0)] private int maxAmount = 64;

    [TextArea(3, 10)]
    [SerializeField] private string description;

    public string Title => title;
    public Sprite Sprite => sprite;
    public int MaxAmount => maxAmount;
    public string Description => description;
}
