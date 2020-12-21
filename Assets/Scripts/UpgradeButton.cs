using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    void Start()
    {
        MainMenu.mainMenu.onUpgradeVehicle += UpdateButton;
    }

    void OnEnable()
    {
        UpdateButton();
    }

    void UpdateButton()
    {
        int id = SceneLoader.sceneLoader.selectedCharacterIndex;
        int grade = PlayerManager.playerManager.playerData.GetVehicleGrade(id);
        DatabaseVehicle databaseVehicle = Resources.Load<DatabaseVehicle>("DatabaseVehicle");
        VehicleScriptableObject vehicleInfo = databaseVehicle.GetVehicleInfoById(id);
        if(grade < vehicleInfo.grade.Count - 1) {
            GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade";
        } else {
            GetComponentInChildren<TextMeshProUGUI>().text = "Max Upgrade";
        }
    }
    void OnDestroy()
    {
        MainMenu.mainMenu.onUpgradeVehicle -= UpdateButton;
    }
}
