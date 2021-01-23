using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageStats : MonoBehaviour
{
    GarageStatItem[] statItems;
    VehicleScriptableObject vehicleInfo;
    VehicleData current;
    VehicleData upgrade;
    List<List<string>> details;

    void Start()
    {
        MainMenu.mainMenu.onUpgradeVehicle += UpdateInfo;
    }
    void OnEnable()
    {
        UpdateInfo();
    }

    void UpdateInfo()
    {
        CollectData();
        BuildData();
        statItems = GetComponentsInChildren<GarageStatItem>();
        for (int i = 0; i < 7; i++) {
            statItems[i].AssignStat(details[i]);
        }
    }

    void CollectData()
    {
        int id = SceneLoader.sceneLoader.selectedCharacterIndex;
        int grade = PlayerManager.playerManager.playerData.GetVehicleGrade(id);
        int nextGrade = PlayerManager.playerManager.playerData.GetVehicleUpgradeGrade(id);
        DatabaseVehicle databaseVehicle = Database.database.databaseVehicle;
        vehicleInfo = databaseVehicle.GetVehicleInfoById(id);
        if(grade != -1) {
            current = vehicleInfo.grade[grade];
        } else {
            current = databaseVehicle.vehicleNotFound;
        }
        upgrade = vehicleInfo.grade[nextGrade];
    }

    void BuildData()
    {
        details = new List<List<string>>(6);
        for(int i = 0; i < 7; i++) {
            List<string> detail = new List<string>( new string[3] );
            details.Add(detail);
        }

        details[0][0] = "Max HP";
        details[1][0] = "Max Ammo";
        details[2][0] = "Damage";
        details[3][0] = "Move Speed";
        details[4][0] = "Bullet Speed";
        details[5][0] = "Reload Rate";
        details[6][0] = "Fire Rate";

        details[0][2] = upgrade.maxHp.ToString();
        details[1][2] = upgrade.maxAmmo.ToString();
        details[2][2] = upgrade.damage.ToString();
        details[3][2] = upgrade.moveSpeed.ToString();
        details[4][2] = upgrade.bulletSpeed.ToString();
        details[5][2] = upgrade.reloadRate.ToString();
        details[6][2] = upgrade.fireRate.ToString();

        if(current.vehicleName == "VehicleNotFound") {
            for(int i = 0; i < 7; i++) {
                details[i][1] = "-";
            }
        } else {
            details[0][1] = current.maxHp.ToString();
            details[1][1] = current.maxAmmo.ToString();
            details[2][1] = current.damage.ToString();
            details[3][1] = current.moveSpeed.ToString();
            details[4][1] = current.bulletSpeed.ToString();
            details[5][1] = current.reloadRate.ToString();
            details[6][1] = current.fireRate.ToString();
        }
    }

    void OnDestroy()
    {
        MainMenu.mainMenu.onUpgradeVehicle -= UpdateInfo;
    }
}
