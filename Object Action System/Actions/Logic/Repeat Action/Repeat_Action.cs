using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Repeat Action", menuName = scriptObjectPath + "Repeat Action")]
public class Repeat_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();

        AddDefaultIntValue("Repeat", 5);
    }
    public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        int repeatAmount = data.GetIntValue("Repeat");
        Debug.Log("repeating");
        for (int i = 0; i < repeatAmount; i++)
        {

            foreach (var action in data.actionValues)
            {
                yield return action.value.Execute(_controller, data, target, hitpoint);
            }
            
        }

        yield break;
    }
}
