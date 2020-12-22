using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageItemRequirement : MonoBehaviour
{
    Inventory itemRequirement;
    VehicleUpgradeData vehicleUpgradeData;
    void Awake()
    {
        itemRequirement = new Inventory();
        MainMenu.mainMenu.onUpgradeVehicle += UpdateRequirements;
    }

    void OnEnable()
    {
        UpdateRequirements();
    }

    void UpdateRequirements()
    {
        if(itemRequirement.items.Count > 0) {
            itemRequirement.items.Clear();
        }
        BuildRequirements();
        ShowRequirements();
    }

    void BuildRequirements()
    {
        int id = SceneLoader.sceneLoader.selectedCharacterIndex;
        int grade = PlayerManager.playerManager.playerData.GetVehicleGrade(id);
        int nextGrade = PlayerManager.playerManager.playerData.GetVehicleUpgradeGrade(id);
        DatabaseVehicleUpgrade databaseVehicleUpgrade = Database.database.databaseVehicleUpgrade;
        if (grade != nextGrade) {
            vehicleUpgradeData = databaseVehicleUpgrade.GetUpgradeRequirement(id, nextGrade);
            foreach (ItemRequirement req in vehicleUpgradeData.requirements) {
                int itemId = req.item.id;
                int haveQuantity = PlayerManager.playerManager.playerData.inventory.GetEntry(itemId).quantity;
                haveQuantity = haveQuantity > 0 ? haveQuantity : 0;
                int requiredQuantity = req.quantity;
                InventoryEntry entry = new InventoryEntry(itemId, haveQuantity, requiredQuantity);
                itemRequirement.items.Add(itemId, entry);
            }
        } else {
            itemRequirement.items.Clear();
        }
    }

    void ShowRequirements()
    {
        GetComponent<ItemUICollection>().AdjustItemCollectionUI(itemRequirement);
    }

    void OnDestroy()
    {
        MainMenu.mainMenu.onUpgradeVehicle -= UpdateRequirements;
    }
}
