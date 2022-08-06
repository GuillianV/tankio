using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AudioManager : MonoBehaviour
{


    //Playlist part
    public PlayList playList;
    private AudioSource PlaylistSource;
    private List<AudioClip> playlistClipsToPlay = new List<AudioClip>();
    private int audioIndex = 0;
    private bool playlistStart = false;
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



    public void StartPlaylist()
    {
        PlaylistSource = gameObject.AddComponent<AudioSource>();
        PlaylistSource.volume = playList.volume;
        PlaylistSource.pitch = (playList.pitch + 1);
        if (playList.isRandom)
        {
            List<AudioClip> clipPartial = new List<AudioClip>();
            playList.clips?.ForEach(c =>
            {
                clipPartial.Add(c);
            });
            for (int i = playList.clips.Count; i > 0; i--)
            {
                int rand = UnityEngine.Random.Range(0, clipPartial.Count);
                playlistClipsToPlay.Add(clipPartial[rand]);
                clipPartial.Remove(clipPartial[rand]);
            }
        }
        else
        {
            playlistClipsToPlay = playList.clips;
        }

        playlistStart = true;
    }
    
    
    void Update () {
        if (!PlaylistSource.isPlaying )
        {
            if (playlistStart && playlistClipsToPlay.Count > 0)
            {
                PlaylistSource.clip = playlistClipsToPlay[audioIndex];
                PlaylistSource.Play();
                audioIndex++;
                if (audioIndex >= playlistClipsToPlay.Count )
                {
                    audioIndex = 0;
                }
            }
            
          
        }
           
    }
    
}
