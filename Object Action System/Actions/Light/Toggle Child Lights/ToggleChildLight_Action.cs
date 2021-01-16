using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Toggle Child Light Components", menuName = scriptObjectPath + "Toggle Child Light Components")]
public class ToggleChildLight_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();
    }

    public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
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

        ToggleLights(targetObject);

        yield break;
    }

    private void ToggleLights(GameObject gameObject)
    {
        if(gameObject)
        {
            foreach (var child in gameObject.GetComponentsInChildren<Light>())
            {
                child.enabled = !child.enabled;                           
            }
        }
    }
}
