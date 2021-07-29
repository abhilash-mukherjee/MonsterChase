using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    public Sound[] sounds;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        foreach(Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }
    private void Start()
    {
        PlaySound("Theme");
    }

    public void PlaySound(string soundName)
    {
        foreach(Sound sound in sounds)
        {
            if(soundName.Equals(sound.name))
            {
                sound.source.Play();
                return;
            }
        }

        Debug.LogWarning("Invalid SoundName");
    }
}
