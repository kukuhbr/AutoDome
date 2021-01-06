using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundsManager : MonoBehaviour
{
    public enum SoundsEnum {
        ui_select,
        ui_back,
        ui_start,
        sfx_vehicle_select,
        vehicle_select_confirm,
        vehicle_engine_revv
    }
    [System.Serializable]
    public class Sound {
        public SoundsEnum type;
        public AudioClip clip;
    }
    [SerializeField]
    public static SoundsManager soundsManager;
    public List<Sound> SFXList;
    public List<Sound> LoopableList;
    public GameObject oneshotPrefab;
    public GameObject loopablePrefab;
    private Dictionary<string, GameObject> playingAudio;
    void Awake()
    {
        soundsManager = this;
        playingAudio = new Dictionary<string, GameObject>();
    }

    private AudioClip SelectAudioClip(SoundsEnum choice, List<Sound> collection)
    {
        foreach (Sound sound in collection) {
            if (sound.type == choice) {
                return sound.clip;
            }
        }
        return null;
    }
    public void PlaySFX(SoundsEnum choice)
    {
        AudioClip audioClip = SelectAudioClip(choice, SFXList);
        if(audioClip == null) return;
        GameObject sfxObject = Instantiate(oneshotPrefab);
        AudioSource source = sfxObject.GetComponent<AudioSource>();
        source.PlayOneShot(audioClip);
        StartCoroutine(DeleteSFXSource(sfxObject));
    }

    IEnumerator DeleteSFXSource(GameObject sfxObject)
    {
        while(sfxObject) {
            AudioSource source = sfxObject.GetComponent<AudioSource>();
            if(!source.isPlaying) {
                Destroy(sfxObject);
            }
            yield return null;
        }
    }

    public void PlayLoop(SoundsEnum choice, string name)
    {
        AudioClip audioClip = SelectAudioClip(choice, LoopableList);
        if(audioClip == null) return;
        GameObject loopObject = Instantiate(loopablePrefab);
        AudioSource source = loopObject.GetComponent<AudioSource>();
        source.clip = audioClip;
        source.Play();
        playingAudio.Add(name, loopObject);
    }

    public void StopLoop(string name)
    {
        if(!playingAudio.ContainsKey(name)) return;
        GameObject loopObject = playingAudio[name];
        AudioSource source = loopObject.GetComponent<AudioSource>();
        source.Stop();
        Destroy(loopObject);
        playingAudio.Remove(name);
    }
}
