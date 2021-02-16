using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Grim.ObjectActionSystem
{
    [CreateAssetMenu(fileName = "Add Attracting Force", menuName = scriptObjectPath + "Add Attracting Force")]
    public class AddAttractingForce_Action : ObjectAction
    {
        public override void Init()
        {
            base.Init();
            SetDescription("Adds a Attracting Force.\nIf self is target then it adds an attracting force to self towards target. \nIf target is chosen it adds an attracting force to target towards self.");
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
                    forceVector = target.transform.position - _controller.transform.position;
                    break;
                case ActionData.GameObjectActionTarget.TARGET:
                    rigidbody = target.GetComponent<Rigidbody>();
                    forceVector = _controller.transform.position - target.transform.position;
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