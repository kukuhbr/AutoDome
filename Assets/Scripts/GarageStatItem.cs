using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GarageStatItem : MonoBehaviour
{
    public void AssignStat(List<string> details)
    {
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        for(int i = 0; i < 3; i++) {
            texts[i].text = details[i];
        }
    }
}
