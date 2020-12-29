using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;
using GoogleMobileAds.Api;

public class RewardScreen : MonoBehaviour
{
    public TextMeshProUGUI rewardParts;
    public PlayerItemHandler playerItemHandler;
    private ItemUICollection itemCollectionHandler;
    [SerializeField]
    private Button adButton;

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
        playerItemHandler.battleInventory.Clear();
        SceneLoader.sceneLoader.LoadScene(SceneIndex.MAIN_MENU);
    }

    public void WatchAd()
    {
        string battleAdId = GoogleMobileAdsScript.adUnitTest;
        RewardedAd battleRewardedAd = GoogleMobileAdsScript.instance.CreateAndLoadRewardedAd(battleAdId);
        if (battleRewardedAd.IsLoaded()) {
            battleRewardedAd.Show();
        }
        battleRewardedAd.OnUserEarnedReward += AdReward;
        //AdReward();
    }

    void AdReward(object sender, EventArgs args)
    {
        int[] items = playerItemHandler.battleInventory.items.Keys.ToArray();
        // Debug.Log("My Items");
        // Debug.Log(items.Length);
        // for(int i = 0; i < items.Length; i++) {
        //     Debug.Log(items[i]);
        // }
        //Debug.Log(items);
        int repetition = UnityEngine.Random.Range(1, 4);
        //Debug.Log(repetition);
        int[] choice = new int[repetition];
        int[] quantity = new int[repetition];
        string notificationText = "You have gained bonus ";
        for(int i = 0; i < repetition; i++) {
            bool inventoryItem = false;
            while(!inventoryItem) {
                choice[i] = UnityEngine.Random.Range(1, items.Length);
                //Debug.Log(choice[i]);
                ItemBase itemData = Database.database.databaseItem.GetItemById(choice[i]);
                inventoryItem = itemData.inInventory;
                if(i < repetition - 1) {
                    notificationText += itemData.itemName + ", ";
                } else {
                    notificationText += itemData.itemName + ".";
                }
                //Debug.Log(inventoryItem);
            }
            quantity[i] = playerItemHandler.battleInventory.GetEntry(choice[i]).quantity;
        }
        //Debug.Log(playerItemHandler.battleInventory.makeTuple());
        playerItemHandler.battleInventory.ForceAdd(new List<int>(choice), new List<int>(quantity));
        //Debug.Log(playerItemHandler.battleInventory.makeTuple());
        Display();
        Destroy(adButton.gameObject);
        Notifier.NotifyBig(notificationText, 2);
    }
}