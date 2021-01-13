using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Toggle Child Light Components", menuName = scriptObjectPath + "Toggle Child Light Components")]
public class ToggleChildLight_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();
        AddDefaultBoolValue("OverrideDisableAll", false);
        AddDefaultBoolValue("OverrideEnableAll", false);
    }

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

        ToggleLights(targetObject, data.GetBoolValue("OverrideDisableAll"), data.GetBoolValue("OverrideEnableAll"));

        yield break;
    }

    private void ToggleLights(GameObject gameObject, bool overrideDisableAll, bool overrideEnableAll)
    {
        if(gameObject)
        {
            foreach (var child in gameObject.GetComponentsInChildren<Light>())
            {
                if (overrideDisableAll)
                {
                    child.enabled = false;
                    continue;
                }
                else if (overrideEnableAll)
                {
                    child.enabled = false;
                    continue;
                }
                else
                {
                    child.enabled = !child.enabled;
                }              
            }
        }
    }
}
