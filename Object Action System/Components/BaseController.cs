using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Grim.ObjectActionSystem
{
    public abstract class BaseController : MonoBehaviour
    {
        public bool showDebugInScene = true;
        public bool compactMode = true;
        public bool IsRunning = false;
        public bool cancelReaction = false;
        public int currentReactionIndex = 0;
        public int currentActionIndex = 0;

        public ObjectReactionData currentReaction;
        public ObjectAction currentAction;

        public bool SetCancelReaction()
        {
            cancelReaction = true;

            return cancelReaction;
        }

        public bool GetIsRunning()
        {
            return IsRunning;
        }
        public void SetIsRunning(bool value)
        {
            IsRunning = value;
        }

        public int GetCurrentActionIndex()
        {
            return currentActionIndex;
        }
        public void SetCurrentActionIndex(int value)
        {
            currentActionIndex = value;
        }

        public ObjectReactionData GetCurrentReaction()
        {
            return currentReaction;
        }

        public void SetCurrentReaction(ObjectReactionData reaction)
        {
            currentReaction = reaction;
        }

        public ObjectAction GetCurrentAction()
        {
            return currentAction;
        }

        public void SetCurrentAction(ObjectAction action)
        {
            currentAction = action;
        }

        public virtual IEnumerator DoReaction(ObjectReactionData reaction, GameObject otherGameObject, Vector3 reactionHitPosition)
        {
            if (cancelReaction)
            {
                StopAllCoroutines();
                SetIsRunning(false);
                yield break;
            }
            SetIsRunning(true);
            SetCurrentReaction(reaction);
            yield return DoActions(otherGameObject, reactionHitPosition);
            SetIsRunning(false);
            yield break;
        }

        public abstract IEnumerator DoActions(GameObject otherGameObject, Vector3 reactionHitPosition);

        public virtual IEnumerator DoAction(ObjectReaction action, GameObject otherGameObject, Vector3 reactionHitPosition)
        {
            ObjectReaction currentReactionAction = action;
            ActionData currentReactionData = currentReactionAction.data;
            if (currentReactionAction.enabled)
            {
                //SetCurrentActionIndex(i);
                SetCurrentAction(currentReactionAction.action);
                yield return currentReactionAction.action.Execute(this, currentReactionData, otherGameObject, reactionHitPosition);
            }

            yield break;
        }

        public abstract List<ObjectReactionData> GetReactions();
    }

}

