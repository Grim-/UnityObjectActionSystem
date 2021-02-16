using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grim.ObjectActionSystem
{
    [CreateAssetMenu(fileName = "Add Buff", menuName = scriptObjectPath + "2222222222Add Buff")]
    public class AddBuff_Action : ObjectAction
    {
        public override void Init()
        {
            base.Init();
            SetDescription("Adds a Buff to Target or Self");
            AddDefaultSOValue("Buff", null);
        }

        public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
        {
            Buffable targetObject = null;
            BaseBuff buff = data.GetSOValue("Buff") as BaseBuff;
            switch (data.targetType)
            {
                case ActionData.GameObjectActionTarget.SELF:
                    targetObject = _controller.GetComponent<Buffable>();
                    break;
                case ActionData.GameObjectActionTarget.TARGET:
                    targetObject = target.GetComponent<Buffable>();

                    break;
            }


            if (buff)
            {
                targetObject.AddBuff(buff.Init(targetObject.gameObject));
            }
            
            yield break;
        }


    }

}