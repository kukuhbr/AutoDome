using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/Item/Usable", order = 2)]
public class ItemUsable : ItemBase {
    public enum UsableType {
        medkit,
        ammokit,
        bomb
    }
    public UsableType usableType;
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