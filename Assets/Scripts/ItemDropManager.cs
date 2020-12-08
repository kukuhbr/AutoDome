using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    public static ItemDropManager instance;
    [SerializeField]
    private GameObject itemHolder;
    public float dropChance = .2f;
    [SerializeField]
    private List<Loot> regularLootTable;
    void Start()
    {
        if(!instance) {
            instance = this;
        }
        StartCoroutine(RegularDrop());
    }

    public void CalculateDrop(Vector3 location, List<Loot> table) {
        float dropRoll = Random.Range(0f, 1f);
        if (dropRoll < dropChance) {
            // Drop From Loot Table
            float itemRoll = Random.Range(0f, 1f);
            foreach (Loot loot in table) {
                if (itemRoll <= loot.chance) {
                    Debug.Log(loot.lootObject.name);
                    Drop(location, loot.lootObject);
                    break;
                }
            }
        }
    }

    void Drop(Vector3 location, ItemBase item) {
        Debug.Log("A Drop is Available");
        GameObject drop = itemHolder;
        drop.GetComponent<ItemHolder>().itemReference = item;
        Instantiate(drop, location, Quaternion.identity);
    }

    IEnumerator RegularDrop() {
        while(true) {
            float timeLeft = 3f;
            while(timeLeft >= 0) {
                timeLeft -= Time.deltaTime;
                yield return null;
            }
            Debug.Log("Coroutine Step!");
            Vector3 randomPos = GetComponent<BattleManager>().SpawnRandomPosition();
            CalculateDrop(randomPos, regularLootTable);
        }

    }
}
