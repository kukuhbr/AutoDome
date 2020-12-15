using UnityEngine;
using System;

[Serializable]
public class Loot {
    public ItemBase lootObject;
    [Range(0f, 1f)]
    public float chance;
}