using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class GunAnimator : MonoBehaviour, ITankAnimator
{
      
      public List<GunAnimation> GunAnimationsList = new List<GunAnimation>();
      private Dictionary<string, GunAnimation> GunAnimationsDico = new Dictionary<string, GunAnimation>();

      

      [System.Serializable]
      public struct GunAnimation
      {
            public string _name;
            public Animator _animator;
            public SpriteRenderer _SpriteRenderer;

      }

      private void Awake()
      {
            GunAnimationsList.ForEach(gunAnimator =>
            {
                  GunAnimationsDico.Add(gunAnimator._name, gunAnimator);
            });
      }
     

      public Animator CallGunAnimator(string gunAnimatorName)
      {
            return GunAnimationsDico[gunAnimatorName]._animator;
      }
   
}
