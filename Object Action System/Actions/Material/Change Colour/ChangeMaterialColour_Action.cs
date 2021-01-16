using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Material Color", menuName = scriptObjectPath + "Change Material Color")]
public class ChangeMaterialColour_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();
        AddDefaultStringValue("PropertyName", "_Color");
        AddDefaultFloatValue("R", 0f);
        AddDefaultFloatValue("G", 0f);
        AddDefaultFloatValue("B", 0f);
    }

    public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        Renderer renderer = null;

        switch (data.targetType)
        {
            case ActionData.GameObjectActionTarget.SELF:
                renderer = _controller.GetComponent<Renderer>();
            break;
            case ActionData.GameObjectActionTarget.TARGET:
                renderer = target.GetComponent<Renderer>();
            break;

        }

        Color newColour = new Color(data.GetFloatValue("R"), data.GetFloatValue("G"), data.GetFloatValue("B"));

        ChangeMaterialColour(_controller, data.GetStringValue("PropertyName"), newColour);
        yield break;
    }

    private void ChangeMaterialColour(BaseController _controller, string propertyName,  Color newColor)
    {
        Renderer renderer = _controller.GetComponent<Renderer>();

        if (renderer)
        {
            renderer.material.SetColor(propertyName, newColor);
        }
        else
        {
            Renderer childRenderer = _controller.GetComponentInChildren<Renderer>();

            if (childRenderer) childRenderer.material.SetColor(propertyName, newColor);
            else
            {
                Debug.LogWarning(_controller.name + " and children have no Renderer Component");
            }
        }
    }
}
