using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TMPro;

public class RewardScreen : MonoBehaviour
{
    public TextMeshProUGUI rewardParts;
    public PlayerItemHandler playerItemHandler;
    private ItemUICollection itemCollectionHandler;

    void Awake()
    {
        itemCollectionHandler = GetComponentInChildren<ItemUICollection>(true);
        Debug.Log(itemCollectionHandler);
    }

    public void Display()
    {
        rewardParts.text = playerItemHandler.battleInventory.GetEntry(9).quantity.ToString();
        // Set inventories
        itemCollectionHandler.AdjustItemCollectionUI(playerItemHandler.battleInventory);
    }

    public void AddBonusParts(int parts)
    {
        playerItemHandler.battleInventory.Add(9, parts);
    }

    public void LoadMenu()
    {
        Tuple<List<int>, List<int>> rewards = playerItemHandler.battleInventory.makeTuple();
        PlayerManager.playerManager.playerData.inventory.Add(rewards.Item1, rewards.Item2);
        SceneLoader.sceneLoader.LoadScene(SceneIndex.MAIN_MENU);
    }

    public void WatchAd()
    {
        //watchad
        Debug.Log("I am watching an ads ;)");
    }
}