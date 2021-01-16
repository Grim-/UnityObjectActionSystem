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
}