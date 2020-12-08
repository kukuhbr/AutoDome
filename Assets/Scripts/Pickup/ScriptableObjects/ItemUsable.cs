using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/Item/Usable", order = 2)]
public class ItemUsable : ItemBase {
    public int strength;
    public bool battleUsable;
    public override void Pickup() {
        if(!handler.Pickup(this)) {
            Use();
        }
    }
    public override void Use() {
        handler.Use(this);
    }
}