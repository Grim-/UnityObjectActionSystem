using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Scale GameObject", menuName = scriptObjectPath + "Scale GameObject")]
public class ScaleGameObject_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();

        AddDefaultFloatValue("Scale", 2f);
    }

    public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        float scaleValue = data.GetFloatValue("Scale");

        switch (data.targetType)
        {
            case ActionData.GameObjectActionTarget.SELF:
                _controller.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            break;
            case ActionData.GameObjectActionTarget.TARGET:
                target.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            break;
        }
        
        yield break;
    }
}
