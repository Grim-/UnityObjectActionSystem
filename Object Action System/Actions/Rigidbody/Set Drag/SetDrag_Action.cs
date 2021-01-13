using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Set Drag", menuName = scriptObjectPath + "Set Drag")]
public class SetrDrag_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();

        AddDefaultFloatValue("DragValue", 0);
    }

    public override IEnumerator Execute(ActionController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        Rigidbody rigidbody = null;

        switch (data.targetType)
        {
            case ActionData.GameObjectActionTarget.SELF:
                rigidbody = _controller.GetComponent<Rigidbody>();
                break;
            case ActionData.GameObjectActionTarget.TARGET:
                rigidbody = target.GetComponent<Rigidbody>();
                break;
        }

        SetAngularDrag(rigidbody, data.GetFloatValue("DragValue"));

        yield break;
    }

    private void SetAngularDrag(Rigidbody rigidbody, float value)
    {
        if (rigidbody) rigidbody.drag = value;
    }
}
