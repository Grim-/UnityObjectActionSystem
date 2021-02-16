namespace Grim.ObjectActionSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Wrapper For the Object Reaction Instance Actions and Data, Is the actual instance of the action and it's data
    /// </summary>
    [System.Serializable]
    public class ObjectReaction
    {
        public bool enabled = true;
        public ObjectAction action;
        public ActionData data;
    }

}