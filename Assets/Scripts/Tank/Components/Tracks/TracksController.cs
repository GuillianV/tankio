using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TracksController : MonoBehaviour, ITankComponent
{


    public SpriteRenderer tracksSpriteLeft;
    public SpriteRenderer tracksSpriteRight;

    private Tracks tracks;
    private float tracksSpeed;
    private float tracksRotationSpeed;

    private void Awake()
    {
        tracks = gameObject.AddComponent<Tracks>();
    }

    void ITankComponent.BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(TracksData))
        {
            TracksData tracksData = (TracksData)obj;
            tracks.LoadData(tracksData);

        }
        else
        {
            Debug.LogError("tracksController cannot load tracksData");
        }


    }

    void ITankComponent.BindComponent()
    {

        if (tracks.Data != null)
        {

            if (tracksSpriteLeft != null && tracksSpriteRight != null &&
          tracks != null)
            {
                tracksSpriteLeft.color = tracks.Data.color;
                tracksSpriteRight.color = tracks.Data.color;
                tracksSpriteLeft.sprite = tracks.Data.spriteTrack;
                tracksSpriteRight.sprite = tracks.Data.spriteTrack;
            }
        }
        else
        {
            Debug.LogError("tracksController cannot load Data in tracks");
        }
    }

    void ITankComponent.BindStats()
    {

        if (tracks.Data != null)
        {
            tracksSpeed = tracks.Data.speed;
            tracksRotationSpeed = tracks.Data.rotationSpeed;
        }
        else
        {
            Debug.LogError("tracksController cannot load Data in tracks");
        }
    }

    public void SetTrackRotationSpeed(float newValue)
    {
        tracksRotationSpeed = newValue;
    }

    public float GetTrackRotationSpeed()
    {
        return tracksRotationSpeed;
    }

    public void SetTrackSpeed(float newValue)
    {
        tracksSpeed = newValue;
    }

    public float GetTrackSpeed()
    {
        return tracksSpeed;
    }

    public TracksData GetBaseData()
    {
        return (TracksData)tracks.GetBaseData();
    }
}
