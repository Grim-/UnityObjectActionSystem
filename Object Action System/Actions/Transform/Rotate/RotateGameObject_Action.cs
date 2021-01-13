using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rotate GameObject", menuName = scriptObjectPath + "Rotate GameObject")]
public class RotateGameObject_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();
        AddDefaultVectorValue("Rotation", Vector3.zero);
    }

    public override IEnumerator Execute(ActionController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        Transform targetTransform = null;

        switch (data.targetType)
        {
            case ActionData.GameObjectActionTarget.SELF:
                targetTransform = _controller.transform;
            break;
            case ActionData.GameObjectActionTarget.TARGET:
                targetTransform = target.transform;
            break;
        }

        Rotate(targetTransform, data.GetVectorValue("Rotation"));

        yield break;
    }

    private void Rotate(Transform transform, Vector3 rotation)
    {
        if(transform) transform.eulerAngles += rotation;
    }
}
