using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Remove Reaction After X Times", menuName = scriptObjectPath + "Remove Reaction After X Times")]
public class RemoveAfter_Actions : ObjectAction
{
    public override void Init()
    {
        base.Init();

        AddDefaultIntValue("Limit", 10);
    }

    public override IEnumerator Execute(ActionController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        if (_controller.GetReactionRuns(_controller.GetCurrentReaction()) >= data.GetIntValue("Limit"))
        {
            _controller.SetCancelReaction();
            _controller.reaction = null;

            yield break;
        }
    }
}
