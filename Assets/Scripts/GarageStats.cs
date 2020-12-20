﻿using System.Collections;
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
        //selected index == ID
        int vehicleIndex = SceneLoader.sceneLoader.selectedCharacterIndex;
        int currentGrade = PlayerManager.playerManager.playerData.vehicleGrades[vehicleIndex];
        DatabaseVehicle databaseVehicle = Resources.Load<DatabaseVehicle>("DatabaseVehicle");
        vehicleInfo = databaseVehicle.GetVehicleInfoById(vehicleIndex);

        current = vehicleInfo.grade[currentGrade];
        currentGrade = currentGrade + 1 > (vehicleInfo.grade.Count - 1) ? currentGrade : currentGrade + 1;
        upgrade = vehicleInfo.grade[currentGrade];
    }

    void BuildData()
    {
        details = new List<List<string>>(6);
        for(int i = 0; i < 7; i++) {
            List<string> detail = new List<string>( new string[3] );
            details.Add(detail);
        }

        details[0][0] = "Max HP";
        details[0][1] = current.maxHp.ToString();
        details[0][2] = upgrade.maxHp.ToString();

        details[1][0] = "Max Ammo";
        details[1][1] = current.maxAmmo.ToString();
        details[1][2] = upgrade.maxAmmo.ToString();

        details[2][0] = "Damage";
        details[2][1] = current.damage.ToString();
        details[2][2] = upgrade.damage.ToString();

        details[3][0] = "Move Speed";
        details[3][1] = current.moveSpeed.ToString();
        details[3][2] = upgrade.moveSpeed.ToString();

        details[4][0] = "Bullet Speed";
        details[4][1] = current.bulletSpeed.ToString();
        details[4][2] = upgrade.bulletSpeed.ToString();

        details[5][0] = "Reload Rate";
        details[5][1] = current.reloadRate.ToString();
        details[5][2] = upgrade.reloadRate.ToString();

        details[6][0] = "Fire Rate";
        details[6][1] = current.fireRate.ToString();
        details[6][2] = upgrade.fireRate.ToString();
    }

    void OnDestroy()
    {
        MainMenu.mainMenu.onUpgradeVehicle -= UpdateInfo;
    }
}
