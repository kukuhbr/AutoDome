using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    public void Buff(ItemBuff.BuffType buffType, float strength, float duration) {
        //List<float> buff = new float {0f, 0f, 0f};
        float[] buff = {0f, 0f, 0f};
        buff[(int)buffType] += strength;
        GetComponent<PlayerScript>().damage += buff[0];
        GetComponent<PlayerScript>().moveSpeed += buff[1];
        GetComponent<PlayerScript>().fireRate -= buff[2];
        StartCoroutine(BuffTimer(duration, buff));
    }

    IEnumerator BuffTimer(float seconds, float[] buff) {
        yield return new WaitForSeconds(seconds);
        GetComponent<PlayerScript>().damage -= buff[0];
        GetComponent<PlayerScript>().moveSpeed -= buff[1];
        GetComponent<PlayerScript>().fireRate += buff[2];
    }

    public bool Pickup(ItemBase item) {
        // Add item to inventory
        return false;
    }

    public void Use(ItemUsable item) {
        if(item.battleUsable) {

        }
    }
}
