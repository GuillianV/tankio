using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class BaseAnimator 
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

    public void BindAnimators(List<BaseAnimatorOverride> overrideControllers)
    {
        AnimationsDico.Clear();
        AnimationsList.ForEach(animation =>
        {
            AnimationsDico.Add(animation._name, animation);
        });

        overrideControllers.ForEach(animator =>
        {
            SetAnimations(animator.name, animator.tracksOverrideController);

        });

    }



    public void SetAnimations(string animatorName, AnimatorOverrideController overrideController)
    {
        AnimationsDico[animatorName]._animator.runtimeAnimatorController = overrideController;
    }

    public Animator CallAnimator(string animatorName)
    {
        return AnimationsDico[animatorName]._animator;
    }

    public void EnableAllAnimators()
    {
        AnimationsList?.ForEach(al =>
        {  if (al._animator == null)return;
            al._animator.enabled = true;
        });
    }
    
    public void DisableAllAnimators()
    {
        AnimationsList?.ForEach(al =>
        {  if (al._animator == null)return;
            al._animator.enabled = false;
        });
    }

    public void ToggleAllAnimators(bool value)
    {
        AnimationsList?.ForEach(al =>
        {
            if (al._animator == null)return;
            al._animator.enabled = value;
        });
    }
    
}
