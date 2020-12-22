using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/Item/Collectable", order = 3)]
public class ItemCollectable : ItemBase {
    public int dropQuantity;
    public override void Pickup() {
        handler.Pickup(this);
    }
    public override void Use() {
        // None
    }
}