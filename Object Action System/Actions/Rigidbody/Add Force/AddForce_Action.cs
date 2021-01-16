using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Add Force", menuName = scriptObjectPath + "Add Force")]
public class AddForce_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();
        AddDefaultFloatValue("ForcePower", 10f);
        AddDefaultVectorValue("ForceVector", Vector3.zero);
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

        AddForce(rigidbody, data.GetVectorValue("ForceVector"), data.GetFloatValue("ForcePower"));
        yield break;
    }

    private void AddForce(Rigidbody rigidbody, Vector3 forceVector, float forcePower)
    {
        if(rigidbody) rigidbody.AddForce(forceVector * forcePower);
    }
}
