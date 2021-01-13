using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Set Mass", menuName = scriptObjectPath + "Set Mass")]
public class SetMass_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();

        AddDefaultFloatValue("MassValue", 0);
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

        SetMass(rigidbody, data.GetFloatValue("MassValue"));

        yield break;
    }

    private void SetMass(Rigidbody rigidbody, float value)
    {
        if (rigidbody) rigidbody.mass = value;
    }
}
