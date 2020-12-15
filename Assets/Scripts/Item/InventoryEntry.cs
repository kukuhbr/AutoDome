public class InventoryEntry {
    public ItemBase item;
    public int quantity;
    public int maxQuantity;
    public float cooldown;
    public InventoryEntry(ItemBase _item, int _quantity, int _maxQuantity) {
        item = _item;
        quantity = _quantity;
        maxQuantity = _maxQuantity;
    }
}
