using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager playerManager;
    PlayerData playerData;
    void Awake()
    {
        playerManager = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Load or create new player data
        playerData = new PlayerData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SavePlayerData()
    {

    }

    public void LoadPlayerData()
    {

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

[Serializable]
public class PlayerData
{
    private List<string> currencyList = new List<string>(new string[] {"bolt", "stars", "gems"});
    private Dictionary<string, int> currencies = new Dictionary<string, int>(); // Bolts, Stars, Gems

    public PlayerData()
    {
        foreach (string currency in currencyList) {
            currencies.Add(currency, 0);
        }
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
        }
    }
}
