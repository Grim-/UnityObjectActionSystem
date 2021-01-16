using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
