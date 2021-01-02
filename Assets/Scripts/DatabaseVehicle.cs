
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/Database/DatabaseVehicle", order = 1)]
public class DatabaseVehicle : ScriptableObject
{
    [SerializeField]
    private List<VehicleScriptableObject> vehicleDB;
    [SerializeField]
    public VehicleData vehicleNotFound = null;

    public VehicleScriptableObject GetVehicleInfoById(int id) {
        for(int i = 0; i < vehicleDB.Count; i++) {
            if (vehicleDB[i].id == id) {
                return vehicleDB[i];
            }
        }
        return null;
    }

    public VehicleData GetVehicleByIdGrade(int id, int grade) {
        for(int i = 0; i < vehicleDB.Count; i++) {
            if (vehicleDB[i].id == id) {
                if(grade < vehicleDB[id].grade.Count) {
                    return vehicleDB[id].grade[grade];
                }
            }
        }
        return vehicleNotFound;
    }
}