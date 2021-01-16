using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Destroy GameObject", menuName = scriptObjectPath + "Destroy GameObject")]
public class DestroyGameObject_Action : ObjectAction
{
    public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        switch (data.targetType)
        {
            case ActionData.GameObjectActionTarget.SELF:
                Destroy(_controller.gameObject);                             
            break;
            case ActionData.GameObjectActionTarget.TARGET:
                Destroy(target);
            break;
        }

        yield break;
    }
}
