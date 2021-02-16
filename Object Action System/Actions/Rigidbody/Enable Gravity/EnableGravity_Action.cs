using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Grim.ObjectActionSystem
{
    [CreateAssetMenu(fileName = "Enable Gravity", menuName = scriptObjectPath + "Enable Gravity")]
    public class EnableGravity_Action : ObjectAction
    {
        public override void Init()
        {
            base.Init();
            SetDescription("Enables self or target's Rigidbody gravity");
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

            EnableGravity(rigidbody);
            yield break;
        }

        private void EnableGravity(Rigidbody rigidbody)
        {
            if (rigidbody != null) rigidbody.useGravity = true;
        }

    } 
}
