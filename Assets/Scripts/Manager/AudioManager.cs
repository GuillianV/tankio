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

            if (sound.objectSource)
            {
                sound.source =   sound.objectSource.AddComponent<AudioSource>();
            }
            else
            {
                sound.source =  gameObject.AddComponent<AudioSource>();
            }
            
           sound.source.clip = sound.clip;
           sound.source.volume = sound.volume;
           sound.source.pitch = sound.pitch;
           sound.source.loop = sound.loop;
        });
    }

    public void Play(string name)
    {
       Sound sound =  sounds.FirstOrDefault(sound => sound.name == name);
       if (sound.source != null)
       {
           sound.source.Play();
       }
       else
       {
           Debug.LogWarning("Sound : "+sound.name+" not found");
       }
      
    }
}
