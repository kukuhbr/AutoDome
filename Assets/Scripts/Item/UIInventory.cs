using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    void OnEnable()
    {
        ItemUICollection coll = GetComponent<ItemUICollection>();
        coll.AdjustItemCollectionUI(PlayerManager.playerManager.playerData.inventory);
    }
}
