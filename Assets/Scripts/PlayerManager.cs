using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager playerManager;
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

    public void IncreaseCurrency(string type, int value)
    {
        int total = playerData.GetCurrencies(type) + value;
        playerData.SetCurrencies(type, total);
    }

    public bool DecreaseCurrency(string type, int value)
    {
        int remainder = playerData.GetCurrencies(type) - value;
        if (remainder < 0) {
            return false;
        }
        playerData.SetCurrencies(type, remainder);
        return true;
    }

    public int GetCurrency(string type)
    {
        return playerData.GetCurrencies(type);
    }
}

public class PlayerData
{
    private List<string> currencyList = new List<string>(new string[] {"bolt", "stars", "gems"});
    private Dictionary<string, int> currencies = new Dictionary<string, int>(); // Bolts, Stars, Gems
    public Inventory inventory;
    public List<int> vehicleGrades;

    public PlayerData()
    {
        foreach (string currency in currencyList) {

            currencies.Add(currency, 0);
        }
        vehicleGrades = new List<int>(new int[] {0, 0, 0});
        inventory = new Inventory();
        // Debug Inventory
        for(int i = 1; i < 10; i++) {
            if (i == 9) {
                inventory.Add(i, 90000);
            } else {
                inventory.Add(i, 20);
            }
        }
        Debug.Log("player inventory size: " + inventory.items.Count);
    }

    public PlayerData(PlayerSave save) {
        DatabaseItem databaseItem = Resources.Load<DatabaseItem>("DatabaseItem");
        vehicleGrades = save.vehicleGrades;
        inventory = new Inventory();
        inventory.Add(save.itemInventoryId, save.itemInventoryQuantity);
    }

    public int GetCurrencies(string type)
    {
        if(currencies.ContainsKey(type)) {
            return currencies[type];
        }
        return 0;
    }
    public void SetCurrencies(string type, int value)
    {
        if(currencies.ContainsKey(type)) {
            currencies[type] = value;
        } else {
            currencies.Add(type, value);
        }
    }
    public int GetVehicleGrade(int id) {
        if(id < vehicleGrades.Count) {
            return vehicleGrades[id];
        }
        return 0;
    }

    public void UpgradeVehicleGrade(int id) {
        if(id < vehicleGrades.Count) {
            DatabaseVehicle databaseVehicle = Resources.Load<DatabaseVehicle>("DatabaseVehicle");
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
}

[Serializable]
public class PlayerSave
{
    public List<int> vehicleGrades;
    public List<int> itemInventoryId;
    public List<int> itemInventoryQuantity;
    public PlayerSave(PlayerData playerData) {
        ClearSaveData();
        vehicleGrades = playerData.vehicleGrades;
        foreach(KeyValuePair<int, InventoryEntry> entry in playerData.inventory.items) {
            itemInventoryId.Add(entry.Key);
            itemInventoryQuantity.Add(entry.Value.quantity);
        }
    }
    void ClearSaveData() {
        vehicleGrades.Clear();
        itemInventoryId.Clear();
        itemInventoryQuantity.Clear();
    }
}
