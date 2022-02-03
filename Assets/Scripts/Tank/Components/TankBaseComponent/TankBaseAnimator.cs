using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class TankBaseAnimator 
{
    public List<Animation> AnimationsList = new List<Animation>();
    private Dictionary<string, Animation> AnimationsDico = new Dictionary<string, Animation>();

      

    [System.Serializable]
    public struct Animation
    {
        public string _name;
        public Animator _animator;
        public SpriteRenderer _SpriteRenderer;

    }

    public void BindAnimators()
    {
        AnimationsList.ForEach(animation =>
        {
            AnimationsDico.Add(animation._name, animation);
        });
    }
     

    public Animator CallAnimator(string animatorName)
    {
        return AnimationsDico[animatorName]._animator;
    }
}
