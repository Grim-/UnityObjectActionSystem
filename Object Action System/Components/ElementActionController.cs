using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementActionController : BaseController
{
    public List<ElementObjectReactionData> reactions;

    public void StartReaction(Element affectorElement, GameObject otherGameobject, Vector3 hitPosition, Collider otherCollider)
    {
        ElementObjectReactionData elementReaction = reactions.Find(x => x.element == affectorElement);

        if (elementReaction != null && !IsRunning)
        {
            StartCoroutine(DoReaction(elementReaction, otherGameobject, hitPosition));
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
