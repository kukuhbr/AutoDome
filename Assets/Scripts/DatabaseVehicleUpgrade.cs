
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

    public bool IsUpgradeAvailable(int id, int grade) {
        int maxGrade = GetVehicleUpgrades(id).grade.Count - 1;
        return (grade <= maxGrade);
    }
}