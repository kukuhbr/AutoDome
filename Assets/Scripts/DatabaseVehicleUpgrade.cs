
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/Database/DatabaseVehicleUpgrade", order = 1)]
public class DatabaseVehicleUpgrade : ScriptableObject
{
    [SerializeField]
    private List<VehicleUpgrade> vehicleUpgradeDB;

    public VehicleUpgradeData GetUpgradeRequirement(int id, int grade) {
        return vehicleUpgradeDB[id].grade[grade];
    }

    public VehicleUpgrade GetVehicleUpgrades(int id) {
        return vehicleUpgradeDB[id];
    }
}