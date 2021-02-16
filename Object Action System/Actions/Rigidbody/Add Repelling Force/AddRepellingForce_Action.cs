namespace Grim.ObjectActionSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    [CreateAssetMenu(fileName = "Add Repelling Force", menuName = scriptObjectPath + "Add Repelling Force")]
    public class AddRepellingForce_Action : ObjectAction
    {
        public override void Init()
        {
            base.Init();
            SetDescription("Adds a Repelling Force.\nIf self is target then it adds a repelling force to self away from target. \nIf target is chosen it adds a repelling force to target away from self.");
            AddDefaultFloatValue("ForcePower", 10f);
            AddDefaultForceMode("ForceMode", ForceMode.Acceleration);
        }

        public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
        {
            Rigidbody rigidbody = null;
            Vector3 forceVector = Vector3.zero;

            switch (data.targetType)
            {
                case ActionData.GameObjectActionTarget.SELF:
                    rigidbody = _controller.GetComponent<Rigidbody>();
                    forceVector = _controller.transform.position - target.transform.position;
                    break;
                case ActionData.GameObjectActionTarget.TARGET:
                    rigidbody = target.GetComponent<Rigidbody>();
                    forceVector = target.transform.position - _controller.transform.position;
                    break;
            }

            AddForce(rigidbody, forceVector, data.GetFloatValue("ForcePower"), data.GetEnumValue("ForceMode"));
            yield break;
        }

        private void AddForce(Rigidbody rigidbody, Vector3 forceVector, float forcePower, ForceMode forcemode)
        {
            if (rigidbody) rigidbody.AddForce(forceVector * forcePower, forcemode);
        }
    }

}