using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionConstraints_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();

        AddDefaultVectorValue("PositionConstraint", Vector3.zero);
    }

    public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
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

        //SetMass(rigidbody, data.GetFloatValue("MassValue"));

        yield break;
    }

    private void SetPositionConstraint(Rigidbody rigidbody, Vector3 value)
    {
        //if (rigidbody) rigidbody.constraints = RigidbodyConstraints.;
    }
}
