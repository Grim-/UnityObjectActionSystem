using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grim.ObjectActionSystem
{
    [CreateAssetMenu(fileName = "Set Is Kinematic", menuName = scriptObjectPath + "Set Is Kinematic")]
    public class SetIsKinematic_Action : ObjectAction
    {
        public override void Init()
        {
            base.Init();
            SetDescription("Set Rigidbody's IsKinematic to IsKinematic");
            AddDefaultBoolValue("IsKinematic", false);
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

            SetKinematic(rigidbody, data.GetBoolValue("IsKinematic"));

            yield break;
        }

        private void SetKinematic(Rigidbody rigidbody, bool value)
        {
            if (rigidbody) rigidbody.isKinematic = value;
        }
    }

}