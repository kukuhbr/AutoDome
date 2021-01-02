using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GarageVehicle : MonoBehaviour
{
    void Start()
    {
        MainMenu.mainMenu.onUpgradeVehicle += UpdateInfo;
    }
    void OnEnable()
    {
        UpdateInfo();
    }

    void UpdateInfo() {
        TextMeshProUGUI vehicleName = GetComponentInChildren<TextMeshProUGUI>();
        DatabaseVehicle databaseVehicle = Database.database.databaseVehicle;
        int id = SceneLoader.sceneLoader.selectedCharacterIndex;
        int grade = PlayerManager.playerManager.playerData.GetVehicleGrade(id);
        VehicleData vehicleData;
        if (grade != -1) {
            vehicleData = databaseVehicle.GetVehicleByIdGrade(id, grade);
        } else {
            vehicleData = databaseVehicle.GetVehicleByIdGrade(id, 0);
        }

        vehicleName.text = vehicleData.vehicleName;
    }

    void OnDestroy()
    {
        MainMenu.mainMenu.onUpgradeVehicle -= UpdateInfo;
    }
}
