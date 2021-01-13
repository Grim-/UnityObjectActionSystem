using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public List<ObjectReactionData> reaction;
    public bool showDebugInScene = true;
    public bool compactMode = true;
    private List<ObjectReactionData> _suspendedReactions;
    private Dictionary<ObjectReactionData, int> _reactionsRunCount;
    private ObjectReactionData currentReaction;
    private bool IsRunning = false;
    private bool cancelReaction = false;
    private int currentActionIndex = 0;
    private ObjectAction currentAction;
    private ActionControllerState controllerState = ActionControllerState.STOPPED;

    void Start()
    {
        SetState(ActionControllerState.INIT);
        _suspendedReactions = new List<ObjectReactionData>();
        _reactionsRunCount = new Dictionary<ObjectReactionData, int>();
    }

    public void StartReaction(GameObject gameObject, Vector3 positionOfReaction, Collider collider)
    {
        if (!IsRunning)
        {
            StartCoroutine(DoReaction(reaction[0], gameObject, positionOfReaction));
        }
    }

    public void StartNamedReaction(string reactionName)
    {
        if (!IsRunning)
        {
            ObjectReactionData react = reaction.Find(x => x.reactionName == reactionName);
            StartCoroutine(DoReaction(react, gameObject, this.transform.position));
        }
    }

    private IEnumerator DoReaction(ObjectReactionData reaction, GameObject otherGameObject, Vector3 reactionHitPosition)
    {
        if (cancelReaction)
        {
            StopAllCoroutines();
            SetIsRunning(false);
            yield break;
        }
        SetIsRunning(true);
        SetState(ActionControllerState.RUNNING);
        SetCurrentReaction(reaction);


        yield return DoActions(otherGameObject, reactionHitPosition);

        SetReactionRunAmount(reaction);
        SetIsRunning(false);
        SetState(ActionControllerState.FINISHED);
        yield break;
    }
    private IEnumerator DoActions(GameObject otherGameObject, Vector3 reactionHitPosition)
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
    public void SetState(ActionControllerState state)
    {
        controllerState = state;
    }
    public ObjectReactionData GetCurrentReaction()
    {
        return currentReaction;
    }
    public ObjectAction GetCurrentAction()
    {
        return currentAction;
    }
    public void SetCurrentReaction(ObjectReactionData reaction)
    {
        currentReaction = reaction;
    }
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
    public void SetCurrentAction(ObjectAction action)
    {
        currentAction = action;
    }
    public int GetCurrentActionIndex()
    {
        return currentActionIndex;
    }
    public void SetCurrentActionIndex(int value)
    {
        currentActionIndex = value;
    }
    public int GetReactionRuns(ObjectReactionData action)
    {
        return _reactionsRunCount[action];
    }
    public void SetReactionRunAmount(ObjectReactionData action)
    {
        if(_reactionsRunCount.ContainsKey(action)) _reactionsRunCount[action]++;
    }
}

public enum ActionControllerState
{
    INIT,
    STOPPED,
    RUNNING,
    PAUSED,
    FINISHED
}
