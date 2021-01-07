using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject cameraRig;
    private Light[] cameraLights;
    private bool characterObtained;
    private bool shouldStopLoopSound;
    private bool characterChanged;
    private float delayedConfirm;

    private void Awake()
    {
        if(scrollbar != null)
        {
            scrollbar.onValueChanged.AddListener(onScrollbar);
        }
        dragHandler = scrollRect.GetComponent<DragHandler>();
        cameraLights = cameraRig.GetComponentsInChildren<Light>();
    }

    private void Start()
    {
        MainMenu.mainMenu.onUpgradeVehicle += VehicleUpgraded;
        dragHandler.DragBegin += SelectingHandler;
        dragHandler.DragEnd += StopSelectingHandler;
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
        foreach(Light light in cameraLights) {
            light.enabled = true;
        }
        characterObtained = true;
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
            characterObtained = PlayerManager.playerManager.playerData.GetVehicleGrade(selectedIndex) != -1;
            characterChanged = true;
            SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_vehicle_select, .4f);
            delayedConfirm = 0.331f;
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
            //Reduce delayed confirm
            if (delayedConfirm > 0) {
                delayedConfirm -= Time.deltaTime;
            }
            //Snap
            float targetValue = (float)selectedIndex/(characters.Count-1);
            if(scrollbar.value >= 0 && scrollbar.value <= 1) {
                if(Mathf.Abs(scrollbar.value - targetValue) > 0.005)
                {
                    float direction = scrollbar.value < targetValue ? 1 : -1;
                    scrollbar.value += 0.005f * direction;
                } else {
                    if (characterChanged) {
                        characterChanged = false;
                        if (characterObtained) {
                            Debug.Log("start revvvvving!");
                            StartCoroutine(PlayConfirmSFX());
                        }
                    }
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

    IEnumerator PlayConfirmSFX() {
        while (delayedConfirm > 0) {
            Debug.Log("Delayed confirm is " + delayedConfirm);
            yield return null;
        }
        Debug.Log("play!");
        SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.ui_vehicle_select_confirm, .4f);
    }

    void SelectingHandler()
    {
        //Debug.Log("Start looping music");
        //shouldStopLoopSound = false;
    }

    void StopSelectingHandler()
    {
        //shouldStopLoopSound = true;
    }

    private void LateUpdate()
    {
        if(characterObtained) {
            inverseRotation += 20*Time.deltaTime;
            selectedCharacter.Rotate(0, 20*Time.deltaTime, 0, Space.World);
            SetLights(true);
        } else {
            SetLights(false);
        }
    }

    void SetLights(bool state)
    {
        foreach (Light light in cameraLights) {
            light.enabled = state;
        }
    }

    void VehicleUpgraded()
    {
        characterObtained = true;
    }

    void OnDestroy()
    {
        if(scrollbar != null)
        {
            scrollbar.onValueChanged.RemoveListener(onScrollbar);
        }
        MainMenu.mainMenu.onUpgradeVehicle -= VehicleUpgraded;
        dragHandler.DragBegin -= SelectingHandler;
        dragHandler.DragEnd -= StopSelectingHandler;
    }
}
