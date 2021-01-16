using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wait for Random Range", menuName = scriptObjectPath + "Wait for Random Range")]
public class WaitForRandomRange_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();

        AddDefaultFloatValue("WaitMin", 1f);
        AddDefaultFloatValue("WaitMax", 3f);
    }

    public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        float waitTime = Random.Range(data.GetFloatValue("WaitMin"), data.GetFloatValue("WaitMax"));
        yield return new WaitForSeconds(waitTime);
    }
}
