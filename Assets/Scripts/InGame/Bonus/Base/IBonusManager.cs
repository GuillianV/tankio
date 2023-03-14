using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBonusManager 
{
        void Bind(BaseScriptableObject scriptableData);

        void BindMapPos(Vector2 globalMapPos);

}
