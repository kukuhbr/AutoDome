using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/Vehicle/VehicleUpgrade", order = 1)]
public class VehicleUpgrade : ScriptableObject
{
    public int id;
    [SerializeField]
    public List<VehicleUpgradeData> grade;
}

[Serializable]
public class VehicleUpgradeData {
    public List<ItemRequirement> requirements;
    public Tuple<List<int>, List<int>> makeTuple() {
        List<int> ids = new List<int>();
        List<int> quantities = new List<int>();
        foreach(ItemRequirement req in requirements) {
            ids.Add(req.item.id);
            quantities.Add(req.quantity);
        }
        return new Tuple<List<int>, List<int>>(ids, quantities);
    }
}

[Serializable]
public class ItemRequirement {
    public ItemBase item;
    public int quantity;
}


