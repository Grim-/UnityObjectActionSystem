using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : BaseController
{
    public List<ObjectReactionData> reactions;

    public void StartReaction(GameObject gameObject, Vector3 positionOfReaction, Collider collider)
    {
        if (!IsRunning)
        {
            if (reactions != null && reactions.Count > 0) StartCoroutine(DoReaction(reactions[0], gameObject, positionOfReaction));
            else Debug.Log("No Reactions Present on Controller");
        }
    }

    public void StartNamedReaction(string reactionName)
    {
        if (!IsRunning)
        {
            ObjectReactionData react = reactions.Find(x => x.reactionName == reactionName);
            if (react != null) StartCoroutine(DoReaction(react, gameObject, this.transform.position));         
            else Debug.Log("No Reaction Found with name " + reactionName);                     
        }
    }

    //private IEnumerator DoReaction(ObjectReactionData reaction, GameObject otherGameObject, Vector3 reactionHitPosition)
    //{
    //    if (cancelReaction)
    //    {
    //        StopAllCoroutines();
    //        SetIsRunning(false);
    //        yield break;
    //    }
    //    SetIsRunning(true);
    //    SetCurrentReaction(reaction);
    //    yield return DoActions(otherGameObject, reactionHitPosition);
    //    SetIsRunning(false);
    //    yield break;
    //}
    public override IEnumerator DoActions(GameObject otherGameObject, Vector3 reactionHitPosition)
    {
        for (int i = 0; i < currentReaction.actions.Count; i++)
        {
            ObjectReaction currentReactionAction = currentReaction.actions[i];
            ActionData currentReactionData = currentReactionAction.data;
            if (currentReactionAction.enabled)
            {
                SetCurrentActionIndex(i);
                SetCurrentAction(currentReactionAction.action);
                yield return currentReactionAction.action.Execute(this, currentReactionData, otherGameObject, reactionHitPosition);
            }
        }
        yield return null;
    }

    //public ObjectReactionData GetCurrentReaction()
    //{
    //    return currentReaction;
    //}
    //public void SetCurrentReaction(ObjectReactionData reaction)
    //{
    //    currentReaction = reaction;
    //}
    //public bool SetCancelReaction()
    //{
    //    cancelReaction = true;

    //    return cancelReaction;
    //}
    //public bool GetIsRunning()
    //{
    //    return IsRunning;
    //}
    //public void SetIsRunning(bool value)
    //{
    //    IsRunning = value;
    //}

    //public ObjectAction GetCurrentAction()
    //{
    //    return currentAction;
    //}
    //public void SetCurrentAction(ObjectAction action)
    //{
    //    currentAction = action;
    //}
    //public int GetCurrentActionIndex()
    //{
    //    return currentActionIndex;
    //}
    //public void SetCurrentActionIndex(int value)
    //{
    //    currentActionIndex = value;
    //}

    //public override IEnumerator DoActions(GameObject otherGameObject, Vector3 reactionHitPosition)
    //{
    //    throw new NotImplementedException();
    //}
}