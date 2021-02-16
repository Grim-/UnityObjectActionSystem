using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grim.ObjectActionSystem
{
    [CreateAssetMenu(fileName = "Add Explosive Force", menuName = scriptObjectPath + "Add Explosive Force")]
    public class AddExplosiveForce_Action : ObjectAction
    {
        public override void Init()
        {
            base.Init();
            SetDescription("Adds Explosion Force to Rigidbody self or target with ForcePower with a ExplosionRadius");

            AddDefaultFloatValue("ForcePower", 100f);
            AddDefaultFloatValue("ExplosionRadius", 0.5f);
        }

        public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
        {
            Rigidbody rigidbody = null;

            float forcePower = data.GetFloatValue("ForcePower");
            float explosionRadius = data.GetFloatValue("ExplosionRadius");

            switch (data.targetType)
            {
                case ActionData.GameObjectActionTarget.SELF:
                    rigidbody = _controller.GetComponent<Rigidbody>();
                    AddExplosiveForce(rigidbody, forcePower, explosionRadius, hitpoint);
                    break;
                case ActionData.GameObjectActionTarget.TARGET:
                    //rigidbody = target.GetComponent<Rigidbody>();

                    Collider[] colliders = Physics.OverlapSphere(_controller.transform.position, explosionRadius, LayerMask.GetMask("Interaction", "Player"));

                    for (int i = 0; i < colliders.Length; i++)
                    {
                        Rigidbody _rigidbody = colliders[i].attachedRigidbody;

                        if (_rigidbody)
                        {
                            _rigidbody.AddForce((_rigidbody.position - _controller.transform.position).normalized * forcePower, ForceMode.VelocityChange);
                        }
                        
                       
                    }
                       
                    break;
            }
            yield break;
        }

        private void AddExplosiveForce(Rigidbody rigidbody, float forcePower, float explosionRadius, Vector3 hitPoint)
        {
            if (rigidbody) rigidbody.AddExplosionForce(forcePower, hitPoint, explosionRadius, 0f, ForceMode.VelocityChange);
        }

    }

}