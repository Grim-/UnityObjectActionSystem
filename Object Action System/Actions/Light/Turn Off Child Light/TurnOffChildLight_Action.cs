using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turn Off Child Lights", menuName = scriptObjectPath + "Turn Off Child Lights")]
public class TurnOffChildLight_Action : ObjectAction
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

        TurnOffLights(targetObject);

        yield break;
    }

    private void TurnOffLights(GameObject gameObject)
    {
        if (gameObject)
        {
            foreach (var child in gameObject.GetComponentsInChildren<Light>())
            {
                child.enabled = false;
            }
        }
    }
}
