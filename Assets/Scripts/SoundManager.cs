using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private string[] soundNames;
    [SerializeField] private AudioClip[] soundClips;

    private Dictionary<string, AudioClip> audioDict;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioDict = new Dictionary<string, AudioClip>();

        for (int i = 0; i < soundNames.Length; i++) {
            audioDict.Add(soundNames[i], soundClips[i]);
        }
    }

    public void PlaySound(string action) {
        try
        {
            audioSource.clip = audioDict[action];
            audioSource.Play();
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
}
