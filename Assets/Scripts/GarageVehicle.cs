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
        DatabaseVehicle databaseVehicle = Resources.Load<DatabaseVehicle>("DatabaseVehicle");
        int id = SceneLoader.sceneLoader.selectedCharacterIndex;
        VehicleData vehicleData = databaseVehicle.GetVehicleByIdGrade(id, PlayerManager.playerManager.playerData.vehicleGrades[id]);
        vehicleName.text = vehicleData.vehicleName;
    }

    void OnDestroy()
    {
        MainMenu.mainMenu.onUpgradeVehicle -= UpdateInfo;
    }
}
