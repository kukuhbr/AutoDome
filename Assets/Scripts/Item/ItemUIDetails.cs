using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUIDetails : MonoBehaviour
{
    Transform[] children;
    bool initialized = false;
    LayoutElement layoutElement;
    public float targetHeight;
    Coroutine InitUI;
    void OnEnable() {
        MainMenu.mainMenu.onItemFocusChange += UpdateDetails;
        layoutElement = GetComponent<LayoutElement>();
        layoutElement.preferredHeight = 0f;
        initialized = false;
        children = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children[0]) {
            if(child != transform) {
                child.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator InitUIDetails(int id) {
        float growTime = .5f;
        float timer = 0f;
        while (timer + Time.deltaTime < growTime) {
            timer += Time.deltaTime;
            float t = timer / growTime;
            t = t * t * (3f - 2f * t);
            float height = Mathf.Lerp(0f, targetHeight, t);
            layoutElement.preferredHeight = height;
            yield return null;
        }
        foreach (Transform child in children) {
            child.gameObject.SetActive(true);
        }
        SetDetailsValues(id);
    }

    void UpdateDetails(int id) {
        if (!initialized) {
            initialized = true;
            InitUI = StartCoroutine(InitUIDetails(id));
        }
        SetDetailsValues(id);
    }

    void SetDetailsValues(int id) {
        DatabaseItem databaseItem = Database.database.databaseItem;
        ItemBase item = databaseItem.GetItemById(id);
        TextMeshProUGUI[] textDetails = GetComponentsInChildren<TextMeshProUGUI>();
        if(textDetails.Length == 2) {
            textDetails[0].text = item.itemName;
            textDetails[1].text = item.description;
        }
    }

    void OnDisable() {
        StopAllCoroutines();
        MainMenu.mainMenu.onItemFocusChange -= UpdateDetails;
    }
}
