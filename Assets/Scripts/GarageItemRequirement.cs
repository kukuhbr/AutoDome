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
        DatabaseVehicleUpgrade databaseVehicleUpgrade = Resources.Load<DatabaseVehicleUpgrade>("DatabaseVehicleUpgrade");
        int maxGrade = databaseVehicleUpgrade.GetVehicleUpgrades(id).grade.Count - 1;
        //grade = grade + 1 > maxGrade ? grade : grade + 1;
        if (grade + 1 <= maxGrade) {
            grade += 1;
            vehicleUpgradeData = databaseVehicleUpgrade.GetUpgradeRequirement(id, grade);
            foreach (ItemRequirement req in vehicleUpgradeData.requirements) {
                int itemId = req.item.id;
                int haveQuantity = PlayerManager.playerManager.playerData.inventory.GetEntry(itemId).quantity;
                haveQuantity = haveQuantity > 0 ? haveQuantity : 0;
                int requiredQuantity = req.quantity;
                Debug.Log(req.item.itemName + " " + itemId + " " + haveQuantity + "/" + requiredQuantity);
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
