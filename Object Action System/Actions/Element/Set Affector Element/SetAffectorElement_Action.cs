using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SetAffectorElement_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();
        AddDefaultSOValue("Element", null);
    }

    public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        Affector targetObject = null;

        switch (data.targetType)
        {
            case ActionData.GameObjectActionTarget.SELF:
                targetObject = _controller.GetComponent<Affector>();
                break;
            case ActionData.GameObjectActionTarget.TARGET:
                targetObject = target.GetComponent<Affector>();
                break;
        }


        targetObject.SetElement(data.GetSOValue("Element") as Element);

        yield break;
    }


}
