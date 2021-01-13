using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Disable Gravity", menuName = scriptObjectPath + "Disable Gravity")]
public class DisableGravity_Action : ObjectAction
{
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

        DisableGravity(rigidbody);
        yield break;
    }

    private void DisableGravity(Rigidbody rigidbody)
    {
        if (rigidbody != null)
        {
            rigidbody.useGravity = false;
        }
    }

}
