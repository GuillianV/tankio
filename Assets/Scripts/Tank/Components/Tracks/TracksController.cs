using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TracksController :  IUpgradable
{


    public SpriteRenderer tracksSpriteLeft;
    public SpriteRenderer tracksSpriteRight;

    private Tracks m_tracks = new Tracks();
    private float tracksSpeed;
    private float tracksRotationSpeed;

    public void BindController(ScriptableObject data)
    {
        BindData(data);
        BindComponent();
        BindStats();
    }

    void BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(TracksData))
        {
            TracksData tracksData = (TracksData)obj;
            m_tracks.LoadData(tracksData);
        }
        else
        {
            Debug.LogError("tracksController cannot load tracksData");
        }


    }

    void BindComponent()
    {

        if (m_tracks.Data != null)
        {

            if (tracksSpriteLeft != null && tracksSpriteRight != null &&
                m_tracks != null)
            {
                tracksSpriteLeft.color = m_tracks.Data.color;
                tracksSpriteRight.color = m_tracks.Data.color;
                tracksSpriteLeft.sprite = m_tracks.Data.spriteTrack;
                tracksSpriteRight.sprite = m_tracks.Data.spriteTrack;
            }
        }
        else
        {
            Debug.LogError("tracksController cannot load Data in tracks");
        }
    }

    void BindStats()
    {

        if (m_tracks.Data != null)
        {
            tracksSpeed = m_tracks.Data.speed;
            tracksRotationSpeed = m_tracks.Data.rotationSpeed;
        }
        else
        {
            Debug.LogError("tracksController cannot load Data in tracks");
        }
    }


    void IUpgradable.Upgrade()
    {
        if (m_tracks.Data != null)
        {

            SetTrackSpeed(GetTrackSpeed() + (m_tracks.Data.coefSpeed * m_tracks.Data.speed));


            SetTrackRotationSpeed(GetTrackRotationSpeed() +  (m_tracks.Data.coefRotationSpeed * m_tracks.Data.rotationSpeed));

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
        return (TracksData)m_tracks.GetBaseData();
    }
}
