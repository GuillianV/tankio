using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCollected : MonoBehaviour
{
    public event EventHandler<MapEvent> Collided;


    public void OnCollided(string tagCollided)
    {
        Collided?.Invoke(this, new MapEvent(tagCollided, transform.position));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        OnCollided(other.gameObject.tag);
    }
}
