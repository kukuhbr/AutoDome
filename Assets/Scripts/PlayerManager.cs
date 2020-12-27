using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager playerManager;
    public static TimeSpan energyFillTime = TimeSpan.FromMinutes(5);
    public PlayerData playerData;
    string saveName = "save";
    void Awake()
    {
        playerManager = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Load or create new player data
        if(Directory.Exists(Application.persistentDataPath + "/saves" + saveName + ".sav")) {
            playerData = LoadPlayerData(saveName);
        } else {
            playerData = new PlayerData();
        }
    }

    public static BinaryFormatter GetBinaryFormatter()
    {
        return new BinaryFormatter();
    }

    public static bool SavePlayerData(PlayerData playerData, string saveName)
    {
        BinaryFormatter formatter = GetBinaryFormatter();
        if (!Directory.Exists(Application.persistentDataPath + "/saves")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }
        string path = Application.persistentDataPath + "/saves/" + saveName + ".sav";
        FileStream file = File.Create(path);
        PlayerSave playerSave = new PlayerSave(playerData);
        formatter.Serialize(file, playerSave);
        file.Close();
        return true;
    }

    public static PlayerData LoadPlayerData(string saveName)
    {
        string path = Application.persistentDataPath + "/saves/" + saveName + ".sav";
        if (!Directory.Exists(path)) {
            return null;
        }
        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);
        try
        {
            PlayerSave save = (PlayerSave)formatter.Deserialize(file);
            PlayerData playerData = new PlayerData(save);
            file.Close();
            return playerData;
        }
        catch
        {
            Debug.LogErrorFormat("Failed to load file at {0}", path);
            file.Close();
            return null;
        }
    }
}

public class PlayerData
{
    public Inventory inventory;
    public List<int> vehicleGrades;
    public DateTime lastEnergyFill;

    public PlayerData()
    {
        vehicleGrades = new List<int>(new int[] {0, 0, 0});
        inventory = new Inventory();
        // Debug Inventory
        for(int i = 1; i < 11; i++) {
            if (i == 9) {
                inventory.Add(i, 15000);
            } else if (i == 10) {
                inventory.Add(i, 0);
            } else {
                inventory.Add(i, 20);
            }
        }
        lastEnergyFill = DateTime.Now;//.Subtract(TimeSpan.FromSeconds(127));
        // Load Test
        // DateTime test = DateTime.Now - TimeSpan.FromMinutes(12);
        // RefillEnergyFrom(test.Ticks);
        Debug.Log("player inventory size: " + inventory.items.Count);
    }

    public PlayerData(PlayerSave save) {
        DatabaseItem databaseItem = Resources.Load<DatabaseItem>("DatabaseItem");
        vehicleGrades = save.vehicleGrades;
        inventory = new Inventory();
        inventory.Add(save.itemInventoryId, save.itemInventoryQuantity);
        lastEnergyFill = new DateTime(save.lastEnergyFill);
    }

    void RefillEnergyFrom(long time) {
        DateTime fromTime = new DateTime(time);
        TimeSpan diff = DateTime.Now - fromTime;
        int passedCycle = diff.Minutes / 5;
        while(!isEnergyMax() && passedCycle > 0) {
            passedCycle -= 1;
            IncreaseEnergy();
        }
        if(!isEnergyMax()) {
            lastEnergyFill = DateTime.Now - TimeSpan.FromTicks(diff.Ticks % PlayerManager.energyFillTime.Ticks);
        }
    }

    public int GetVehicleGrade(int id) {
        return vehicleGrades[id];
    }

    public int GetVehicleUpgradeGrade(int id) {
        int vehicleGrade = vehicleGrades[id] + 1 > GetVehicleMaxGrade(id) ? vehicleGrades[id] : vehicleGrades[id] + 1;
        return vehicleGrade;
    }

    public int GetVehicleMaxGrade(int id) {
        int maxGrade = Database.database.databaseVehicleUpgrade.GetVehicleUpgrades(id).grade.Count - 1;
        return maxGrade;
    }

    public void UpgradeVehicleGrade(int id) {
        if(id < vehicleGrades.Count) {
            DatabaseVehicle databaseVehicle = Database.database.databaseVehicle;
            VehicleScriptableObject vehicleInfo = databaseVehicle.GetVehicleInfoById(id);
            if(vehicleGrades[id] < vehicleInfo.grade.Count - 1) {
                vehicleGrades[id] += 1;
            }
        }
    }

    public bool VehicleUpgradeAvailable(int id) {
        int grade = vehicleGrades[id];
        bool isUpgradeAvailable = Database.database.databaseVehicleUpgrade.IsUpgradeAvailable(id, grade+1);
        return isUpgradeAvailable;
    }

    public bool VehicleUpgradeHaveItems(int id) {
        int grade = vehicleGrades[id];
        VehicleUpgradeData vehicleUpgradeData = Database.database.databaseVehicleUpgrade.GetUpgradeRequirement(id, grade+1);
        List<int> itemIds = new List<int>();
        List<int> itemQuantities = new List<int>();
        foreach (ItemRequirement req in vehicleUpgradeData.requirements) {
            itemIds.Add(req.item.id);
            itemQuantities.Add(req.quantity);
        }
        bool haveItems = PlayerManager.playerManager.playerData.inventory.HaveItems(itemIds, itemQuantities);
        return (haveItems);
    }

    public void VehicleUpgradeRemoveItems(int id) {
        int grade = vehicleGrades[id];
        VehicleUpgradeData vehicleUpgradeData = Database.database.databaseVehicleUpgrade.GetUpgradeRequirement(id, grade+1);
        Tuple<List<int>, List<int>> reqs = vehicleUpgradeData.makeTuple();
        inventory.Remove(reqs.Item1, reqs.Item2);
    }

    public Inventory DisplayInventory() {
        Inventory visible = new Inventory();
        foreach(KeyValuePair<int, InventoryEntry> entry in inventory.items) {
            ItemBase item = Database.database.databaseItem.GetItemById(entry.Value.id);
            if(item.inInventory) {
                visible.Add(entry.Value);
            }
        }
        return visible;
    }

    public bool IncreaseEnergy() {
        if(!isEnergyMax()) {
            inventory.Add(10, 1);
            lastEnergyFill = DateTime.Now;
            Debug.Log("Increased to "+inventory.GetEntry(10).quantity);
            return true;
        }
        return false;
    }

    public bool DecreaseEnergy() {
        if(isEnergyEnough()) {
            inventory.Remove(10, 1);
            Debug.Log("Decreased to "+inventory.GetEntry(10).quantity);
            return true;
        }
        return false;
    }

    public bool isEnergyEnough() {
        return inventory.GetEntry(10).quantity > 0;
    }

    public bool isEnergyMax() {
        InventoryEntry energyEntry = inventory.GetEntry(10);
        return energyEntry.quantity == energyEntry.maxQuantity;
    }
}

[Serializable]
public class PlayerSave
{
    public List<int> vehicleGrades;
    public List<int> itemInventoryId;
    public List<int> itemInventoryQuantity;
    public long lastEnergyFill;
    public PlayerSave(PlayerData playerData) {
        ClearSaveData();
        vehicleGrades = playerData.vehicleGrades;
        Tuple<List<int>, List<int>> itemTuple = playerData.inventory.makeTuple();
        itemInventoryId = itemTuple.Item1;
        itemInventoryId = itemTuple.Item2;
        // foreach(KeyValuePair<int, InventoryEntry> entry in playerData.inventory.items) {
        //     itemInventoryId.Add(entry.Key);
        //     itemInventoryQuantity.Add(entry.Value.quantity);
        // }
        lastEnergyFill = playerData.lastEnergyFill.Ticks;
    }
    void ClearSaveData() {
        vehicleGrades.Clear();
        itemInventoryId.Clear();
        itemInventoryQuantity.Clear();
    }
}
