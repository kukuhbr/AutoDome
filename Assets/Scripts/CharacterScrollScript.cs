using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterScrollScript : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Scrollbar scrollbar;
    public float minPos;
    public float maxPos;
    public float elasticTolerance;
    public List<Transform> characters;
    private Transform selectedCharacter;
    private int selectedIndex;
    private float inverseRotation;
    private DragHandler dragHandler;

    private void Awake()
    {
        if(scrollbar != null)
        {
            scrollbar.onValueChanged.AddListener(onScrollbar);
        }
        dragHandler = scrollRect.GetComponent<DragHandler>();
    }

    private void Start()
    {
        characters = new List<Transform>();
        foreach (Transform child in transform)
        {
            characters.Add(child);
        }
        selectedCharacter = characters[0];
        selectedIndex = 0;
        selectedCharacter.localScale = new Vector3(7,7,7);
        minPos = 0;
        maxPos = (characters.Count - 1) * 40;
        inverseRotation = 0;
        elasticTolerance = 20;
    }

    private void onScrollbar(float value)
    {
        if(0 <= scrollbar.value && scrollbar.value <= 1)
        {
            transform.position = new Vector3(-Mathf.Lerp(minPos, maxPos, value), transform.position.y, transform.position.z);
        }
        // else {
        //     float elasticPosition = -Mathf.Clamp(transform.position.x + (100 * scrollbar.value % 1),minPos-elasticTolerance, maxPos+elasticTolerance);
        //     transform.position = new Vector3(elasticPosition, transform.position.y, transform.position.z);
        // }
        HighlightPlayer();
    }

    private void HighlightPlayer() {
        float posAlignment = this.transform.position.x * -1 / 40;
        int targetAlignment = Mathf.Clamp(Mathf.RoundToInt(posAlignment), 0, characters.Count-1);
        if (targetAlignment != selectedIndex)
        {
            selectedCharacter.localScale = new Vector3(5,5,5);
            selectedCharacter.Rotate(0, -inverseRotation, 0, Space.World);
            inverseRotation = 0;
            selectedIndex = targetAlignment;
            selectedCharacter = characters[selectedIndex];
            selectedCharacter.localScale = new Vector3(7,7,7);
        }
        // if(this.transform.position.x % 40 != 0)
        // {
        //     transform.position = new Vector3(-targetAlignment * 40f, transform.position.y, transform.position.z);
        //     Debug.Log("Pos Alignment: " + posAlignment);
        //     Debug.Log("Target Position: " + targetAlignment);
        // }
    }

    private void Update()
    {
        SceneLoader.sceneLoader.selectedCharacterIndex = selectedIndex;
        if(!dragHandler.isDrag)
        {
            //Snap
            float targetValue = (float)selectedIndex/(characters.Count-1);
            if(scrollbar.value >= 0 && scrollbar.value <= 1) {
                if(Mathf.Abs(scrollbar.value - targetValue) > 0.005)
                {
                    float direction = scrollbar.value < targetValue ? 1 : -1;
                    scrollbar.value += 0.005f * direction;
                }
            }
            // else {
            //     if(Mathf.Abs(scrollbar.value - targetValue) > 0.005)
            //     {
            //         float direction = scrollbar.value < 0 ? 1 : -1;
            //         scrollbar.value += 0.005f * direction;
            //     }
            // }

        }
    }

    private void LateUpdate()
    {
        inverseRotation += 20*Time.deltaTime;
        selectedCharacter.Rotate(0, 20*Time.deltaTime, 0, Space.World);

    }

    void OnDestroy()
    {
        if(scrollbar != null)
        {
            scrollbar.onValueChanged.RemoveListener(onScrollbar);
        }
    }
}
