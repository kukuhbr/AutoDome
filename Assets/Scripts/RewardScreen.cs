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
    private RewardedAd bonusDrop;
    private bool isAdLoaded;

    void Awake()
    {
        itemCollectionHandler = GetComponentInChildren<ItemUICollection>(true);
    }

    void Start()
    {
        SoundsManager.soundsManager.PlayLoop(SoundsManager.SoundsEnum.music_gameover, "music_gameover", 1f);
        SoundsManager.soundsManager.StopLoop("music_battle");

        bonusDrop = new RewardedAd(GoogleMobileAdsScript.itemsAdId);
        //bonusDrop.OnAdLoaded += AdLoaded;
        bonusDrop.OnAdFailedToLoad += FailToLoad;
        bonusDrop.OnAdFailedToShow += HandleAdFailToShow;
        bonusDrop.OnUserEarnedReward += AdReward;

        AdRequest request =  new AdRequest.Builder().Build();
        bonusDrop.LoadAd(request);
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
        PlayerData playerData = PlayerManager.playerManager.playerData;
        Tuple<List<int>, List<int>> rewards = playerItemHandler.battleInventory.makeTuple();
        playerData.inventory.Add(rewards.Item1, rewards.Item2);
        playerItemHandler.battleInventory.Clear();
        playerData.battleSlot.ChangeReferenceInventory(playerData.inventory);
        PlayerManager.SavePlayerData(playerData);
        SceneLoader.sceneLoader.LoadScene(SceneIndex.MAIN_MENU);
        SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_select);
        SoundsManager.soundsManager.StopLoop("music_gameover");
    }

    public void WatchAd()
    {
        SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_select);
        if (bonusDrop.IsLoaded()) {
            bonusDrop.Show();
        } else {
            Notifier.NotifyBig("Ad is not yet Loaded", 2);
        }
    }

    void FailToLoad(object sender, AdErrorEventArgs args)
    {
        Notifier.NotifyBig("Ad Failed to Load", 2);
        Debug.Log("HandleFailedToReceiveAd event received with message: "
                        + args.Message);
    }

    void HandleAdFailToShow(object sender, EventArgs args)
    {
        Notifier.NotifyBig("Ad Failed to Show", 2);
        Debug.Log("ad failed to show");
    }

    void AdReward(object sender, EventArgs args)
    {
        StartCoroutine(AdRewardCoroutine());
    }

    System.Collections.IEnumerator AdRewardCoroutine()
    {
        yield return new WaitForEndOfFrame();
        int[] items = playerItemHandler.battleInventory.items.Keys.ToArray();
        int repetition = UnityEngine.Random.Range(1, 4);
        int[] choice = new int[repetition];
        int[] quantity = new int[repetition];
        string notificationText = "You have gained bonus ";
        ItemBase itemData;
        for(int i = 0; i < repetition; i++) {
            bool validReward = false;
            int rollTries = 0;
            while(!validReward) {
                rollTries += 1;
                if (rollTries > 10) break;
                choice[i] = items[UnityEngine.Random.Range(0, items.Length)];
                itemData = Database.database.databaseItem.GetItemById(choice[i]);
                validReward = itemData.inInventory;
                validReward = validReward && playerItemHandler.battleInventory.GetEntry(choice[i]).quantity > 0;
                if(!validReward) continue;
            }
            int ownedQuantity = playerItemHandler.battleInventory.GetEntry(choice[i]).quantity;
            quantity[i] = ownedQuantity > 5 ? 5 : ownedQuantity;
            if (rollTries > 10) {
                choice[i] = 9;
                quantity[i] = UnityEngine.Random.Range(10, 31);
            }
            itemData = Database.database.databaseItem.GetItemById(choice[i]);
            if (i == 0) {
                notificationText += quantity[i] + " " + itemData.itemName;
            } else {
                notificationText += ", " + quantity[i] + " " + itemData.itemName;
            }
            if (i == repetition - 1) {
                notificationText += ".";
            }
        }
        playerItemHandler.battleInventory.ForceAdd(new List<int>(choice), new List<int>(quantity));
        Display();
        Destroy(adButton.gameObject);
        Notifier.NotifyBig(notificationText, 2);
    }

    void OnDestroy()
    {
        if (bonusDrop != null) {
            bonusDrop.OnAdFailedToLoad -= FailToLoad;
            bonusDrop.OnAdFailedToShow -= HandleAdFailToShow;
            bonusDrop.OnUserEarnedReward -= AdReward;
        }
    }
}