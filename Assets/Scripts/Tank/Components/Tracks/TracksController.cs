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

    public void BindController(TracksData tracksData)
    {
        m_tracks.LoadData(tracksData);
        BindComponent();
        BindStats();
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
                if(m_tracks.Data.TracksSpriteScaleX != 0 &&  m_tracks.Data.TracksSpriteScaleY != 0)
                {
                    tracksSpriteLeft.size = new Vector2(m_tracks.Data.TracksSpriteScaleX, m_tracks.Data.TracksSpriteScaleY);
                    tracksSpriteRight.size = new Vector2(m_tracks.Data.TracksSpriteScaleX, m_tracks.Data.TracksSpriteScaleY);

                }
                
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
