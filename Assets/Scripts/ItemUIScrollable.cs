using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIScrollable : MonoBehaviour
{

    LayoutElement layoutElement;
    bool adjusted = false;
    public float initialHeight;
    public float targetHeight;
    void OnEnable()
    {
        MainMenu.mainMenu.onItemFocusChange += AdjustUISize;
        layoutElement = GetComponent<LayoutElement>();
        layoutElement.preferredHeight = initialHeight;
        adjusted = false;
    }

    void AdjustUISize(int id) {
        if (!adjusted) {
            adjusted = true;
            StartCoroutine(ShrinkLayoutSize());
        }
    }

    IEnumerator ShrinkLayoutSize() {
        float growTime = .5f;
        float timer = 0f;
        while (timer + Time.deltaTime < growTime) {
            timer += Time.deltaTime;
            float t = timer / growTime;
            t = t * t * (3f - 2f * t);
            float height = Mathf.Lerp(initialHeight, targetHeight, t);
            layoutElement.preferredHeight = height;
            yield return null;
        }
    }

    void OnDestroy()
    {
        MainMenu.mainMenu.onItemFocusChange -= AdjustUISize;
    }
}