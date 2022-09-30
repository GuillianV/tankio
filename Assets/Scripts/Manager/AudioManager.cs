using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;



public class AudioManager : MonoBehaviour
{


    //Playlist part
    public PlayList playList;

    public GameObject playListParent;
    public List<Sound> sounds;

    private AudioSource PlaylistSource;
    private List<MegaClip> playlistClipsToPlay = new List<MegaClip>();
    private int audioIndex;
    private bool playlistStart = false;
    private AudioSource audioSourceActual;

    private bool priorityUp;
    private bool priorityDown;
    private float actualVolumeUp = 0;
    private float actualVolumeDown = 0.5f;
    
    
    public int actualPriority;

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



    public void InitPlaylist()
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
           
                int minPriorite = -1;
                int maxPriorite = -1;

                GameObject Child = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity, playListParent.transform);
                Child.name =  clipPartial[rand].name;


                clipPartial[rand].megaClips?.ForEach(megaClipsPart =>
                {
                    if (minPriorite == -1)
                        minPriorite = megaClipsPart.priority;

                    if (maxPriorite == -1)
                        maxPriorite = megaClipsPart.priority;

                    if (megaClipsPart.priority > maxPriorite)
                        maxPriorite = megaClipsPart.priority;

                    if (megaClipsPart.priority < minPriorite)
                        minPriorite = megaClipsPart.priority;


                    megaClipsPart.audioClip.source  = Child.AddComponent<AudioSource>();

                });

                clipPartial[rand].maxPriority = maxPriorite;
                clipPartial[rand].minPriority = minPriorite;
                clipPartial[rand].actualPriority = minPriorite;

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

                actualPriority = playlistClipsToPlay[audioIndex].actualPriority;

                playlistClipsToPlay[audioIndex].megaClips?.ForEach(megaClipPart =>
                {
                    megaClipPart.audioClip.source.clip = megaClipPart.audioClip.clip;
                    megaClipPart.audioClip.source.pitch =   megaClipPart.audioClip.pitch;

                    if (megaClipPart.priority <= playlistClipsToPlay[audioIndex].actualPriority)
                        megaClipPart.audioClip.source.volume = megaClipPart.audioClip.volume;
                    else
                        megaClipPart.audioClip.source.volume = 0;


                    
                    megaClipPart.audioClip.source.Play();

                });
                audioSourceActual = playlistClipsToPlay[audioIndex]?.megaClips[0]?.audioClip.source;


                audioIndex++;
                if (audioIndex >= playlistClipsToPlay.Count)
                {
                    audioIndex = 0;
                }
            }
        }

        
        UpdatePriority();


    }


    public void PriorityUp()
    {

      
        if (playlistClipsToPlay.Count <= 0)
            return;
        
        priorityUp = true;
        priorityDown = false;

        
        MegaClip megaClipUp = playlistClipsToPlay[audioIndex];
        megaClipUp.actualPriority+=1;
            
        if (megaClipUp.actualPriority >= megaClipUp.maxPriority || megaClipUp.actualPriority < megaClipUp.minPriority)
            megaClipUp.actualPriority = megaClipUp.maxPriority;


    }

    public void PriorityDown()
    {
        
      
        if (playlistClipsToPlay.Count <= 0)
            return;
        
        priorityUp = false;
        priorityDown = true ;

        
        MegaClip megaClipDown = playlistClipsToPlay[audioIndex];

        if (megaClipDown.actualPriority > megaClipDown.maxPriority || megaClipDown.actualPriority <= megaClipDown.minPriority)
            return;
        megaClipDown.actualPriority -= 1;

    }

    private void UpdatePriority()
    {
    

        MegaClip megaClip = playlistClipsToPlay[audioIndex];
        if (priorityUp)
        {
            
            megaClip.megaClips?.ForEach(megaClipPart =>
            {
                if (megaClipPart.priority == megaClip.actualPriority)
                {
                    megaClipPart.audioClip.source.volume = Mathf.Lerp(megaClipPart.audioClip.source.volume, megaClipPart.audioClip.volume, 0.5f * Time.deltaTime);
             
                    if (megaClipPart.audioClip.source.volume == megaClipPart.audioClip.volume)
                    {
                        priorityUp = false;
                    }
                }
                    
               

            });
            
        }else if (priorityDown)
        {
          
            megaClip.megaClips?.ForEach(megaClipPart =>
            {
                if (megaClipPart.priority == megaClip.actualPriority + 1)
                {
                    megaClipPart.audioClip.source.volume  =  Mathf.Lerp(megaClipPart.audioClip.source.volume , 0f, 0.5f * Time.deltaTime);
             
                    if (megaClipPart.audioClip.source.volume == 0)
                    {
                        priorityUp = false;
                    }
                }
                
            });
       

        }
    }
    
}
