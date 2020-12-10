using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/Item/Buff", order = 1)]
public class ItemBuff : ItemBase {
    public enum BuffType {
        damage,
        moveSpeed,
        fireRate
    }
    public BuffType buffType;
    public float duration;
    public float strength;
    public override void Pickup() {
        Use();
    }
    public override void Use() {
        handler.Buff(this);//buffType, strength, duration);
    }

}