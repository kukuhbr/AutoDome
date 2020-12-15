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
}

[Serializable]
public class ItemRequirement {
    public ItemBase item;
    public int quantity;
}


