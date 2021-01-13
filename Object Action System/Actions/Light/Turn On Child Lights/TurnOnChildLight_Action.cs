using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turn On Child Lights", menuName = scriptObjectPath + "Turn On Child Lights")]
public class TurnOnChildLight_Action : ObjectAction
{
    public override IEnumerator Execute(ActionController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        GameObject targetObject = null;

        switch (data.targetType)
        {
            case ActionData.GameObjectActionTarget.SELF:
                targetObject = _controller.gameObject;
                break;
            case ActionData.GameObjectActionTarget.TARGET:
                targetObject = target.gameObject;
                break;
        }

        TurnOnLights(targetObject);

        yield break;
    }

    private void TurnOnLights(GameObject gameObject)
    {
        if (gameObject)
        {
            foreach (var child in gameObject.GetComponentsInChildren<Light>())
            {
                child.enabled = true;
            }
        }
    }
}
