using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public List<Sound> sounds;

    // Start is called before the first frame update
    private void Awake()
    {
        sounds.ForEach(sound =>
        {
           sound.source =  gameObject.AddComponent<AudioSource>();
           sound.source.clip = sound.clip;
           sound.source.volume = sound.volume;
           sound.source.pitch = sound.pitch;
           sound.source.loop = sound.loop;
        });
    }

    public void Play(string name)
    {
       Sound sound =  sounds.FirstOrDefault(sound => sound.name == name);
       sound.source.Play();
    }
}
