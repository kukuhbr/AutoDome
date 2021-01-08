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
    private float regularDropInterval;
    [SerializeField]
    private List<Loot> regularLootTable;
    private bool gameOver;
    void Start()
    {
        if(!instance) {
            instance = this;
        }
        StartCoroutine(RegularDrop());
        gameOver = false;
        BattleEvents.battleEvents.onGameOver += GameOver;
    }

    public void CalculateDrop(Vector3 location, List<Loot> table) {
        float dropRoll = Random.Range(0f, 1f);
        if (dropRoll < dropChance) {
            // Drop From Loot Table
            float itemRoll = Random.Range(0f, 1f);
            foreach (Loot loot in table) {
                if (itemRoll <= loot.chance) {
                    Drop(location, loot.lootObject);
                    break;
                }
            }
        }
    }

    void Drop(Vector3 location, ItemBase item) {
        GameObject drop = itemHolder;
        drop.GetComponent<ItemHolder>().itemReference = item;
        Instantiate(drop, location, Quaternion.identity);
        SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.item_drop);
    }

    void GameOver()
    {
        gameOver = true;
    }

    IEnumerator RegularDrop() {
        while(!gameOver) {
            float timeLeft = regularDropInterval;
            while(timeLeft >= 0) {
                timeLeft -= Time.deltaTime;
                yield return null;
            }
            Vector3 randomPos = GetComponent<BattleManager>().SpawnRandomPosition();
            CalculateDrop(randomPos, regularLootTable);
        }

    }

    void OnDestroy()
    {
        BattleEvents.battleEvents.onGameOver -= GameOver;
    }
}
