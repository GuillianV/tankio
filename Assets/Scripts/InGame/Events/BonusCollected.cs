using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCollected : MonoBehaviour
{
    public event EventHandler<TagEvent> Collided;


    public void OnCollided(string tagCollided)
    {
        Collided?.Invoke(this, new TagEvent(tagCollided));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        OnCollided(other.gameObject.tag);
    }
}
