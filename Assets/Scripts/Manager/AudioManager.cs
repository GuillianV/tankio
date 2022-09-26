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
    private List<MegaClip> playlistClipsToPlay = new List<MegaClip>();
    private int audioIndex = 0;
    private bool playlistStart = false;
    private AudioSource audioSourceActual;
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
        Sound sound = sounds.FirstOrDefault(sound => sound.name == name);
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

        if (playList.isRandom)
        {
            List<MegaClip> clipPartial = new List<MegaClip>();
            playList.clips?.ForEach(c =>
            {
                clipPartial.Add(c);
            });
            for (int i = playList.clips.Count; i > 0; i--)
            {
                int rand = UnityEngine.Random.Range(0, clipPartial.Count);
               

                int minPriorite = 0;
                int maxPriorite = 0;

                clipPartial[rand].megaClips?.ForEach(megaClipsPart =>
                {
                    if (megaClipsPart.priority > maxPriorite)
                        maxPriorite = megaClipsPart.priority;

                    if (megaClipsPart.priority < minPriorite)
                        minPriorite = megaClipsPart.priority;


                    megaClipsPart.audioClip.source  = gameObject.AddComponent<AudioSource>();

                });

                clipPartial[rand].maxPriority = maxPriorite;
                clipPartial[rand].minPriority = minPriorite;


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




    void Update()
    {



        if (playlistStart && playlistClipsToPlay.Count > 0)
        {

            if (audioSourceActual == null)
                audioSourceActual = playlistClipsToPlay[audioIndex]?.megaClips[0]?.audioClip.source;


            if (!audioSourceActual.isPlaying)
            {



                playlistClipsToPlay[audioIndex].megaClips?.ForEach(megaClipPart =>
                {
                    megaClipPart.audioClip.source.clip = megaClipPart.audioClip.clip;
                    megaClipPart.audioClip.source.volume = megaClipPart.audioClip.volume;
                    megaClipPart.audioClip.source.pitch =   megaClipPart.audioClip.pitch;
                    megaClipPart.audioClip.source.Play();

                });
                playlistClipsToPlay[audioIndex].playing = true;
                audioSourceActual = playlistClipsToPlay[audioIndex]?.megaClips[0]?.audioClip.source;


                audioIndex++;
                if (audioIndex >= playlistClipsToPlay.Count)
                {
                    audioIndex = 0;
                }
            }
        }


    }

}
