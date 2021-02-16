using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grim.ObjectActionSystem
{

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
            int count = 0;
            int repeatAmount = data.GetIntValue("Repeat");
            Debug.Log("repeating");
            Debug.Log(_controller.GetReactions()[_controller.currentReactionIndex]);

            for (int i = 0; i < repeatAmount; i++)
            {
                count++;

                yield return _controller.DoAction(_controller.GetReactions()[_controller.currentReactionIndex].actions[5], target, hitpoint);
            }

            yield break;
        }
    }
}