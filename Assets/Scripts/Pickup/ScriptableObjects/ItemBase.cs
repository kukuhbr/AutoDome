using UnityEngine;

public abstract class ItemBase : ScriptableObject {//, IPickable {
    public string itemName;
    [Multiline]
    public string description;
    public Sprite icon;
    public GameObject model;
    protected PlayerItemHandler handler;
    public void SetHandler(GameObject obj) {
        handler = obj.GetComponent<PlayerItemHandler>();
    }
    public abstract void Pickup();
    public abstract void Use();
}