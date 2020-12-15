
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/Database/DatabaseItem", order = 1)]
public class DatabaseItem : ScriptableObject
{
    [SerializeField]
    private List<ItemBase> itemDB;
    [SerializeField]
    private ItemBase itemNotFound = null;

    public ItemBase GetItemById(int id)
    {
        for(int i = 0; i < itemDB.Count; i++) {
            if (itemDB[i].id == id) {
                return itemDB[i];
            }
        }
        return itemNotFound;
    }

    public ItemBase GetItemByName(string name) {
        for(int i = 0; i < itemDB.Count; i++) {
            if (itemDB[i].name == name) {
                return itemDB[i];
            }
        }
        return itemNotFound;
    }
}