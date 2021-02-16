namespace Grim.ObjectActionSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    /// <summary>
    ///  Defines a Reaction, a list of actions with data to be executed
    /// </summary>
    [System.Serializable]
    public class ObjectReactionData
    {
        public string reactionName;
        public List<ObjectReaction> actions;
        [HideInInspector]
        public GameObject parent;

        public void Init(GameObject _parent)
        {
            parent = _parent;
        }
    }

    //  
}