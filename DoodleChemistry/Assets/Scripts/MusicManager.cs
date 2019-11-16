using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource sfxManager;
    [SerializeField] private AudioClip[] sfxClips;
    // Start is called before the first frame update
    void Awake()
    {
        // component version of a singleton
        if (instance != null) return;

        instance = this;
        DontDestroyOnLoad(gameObject);
        GetComponent<AudioSource>().Play();
    }

    public void PlaySoundEffect(int index)
    {
        sfxManager.clip = sfxClips[index];
        sfxManager.Play();
    }
}
