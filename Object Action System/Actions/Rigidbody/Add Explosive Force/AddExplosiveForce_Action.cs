using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Add Explosive Force", menuName = scriptObjectPath + "Add Explosive Force")]
public class AddExplosiveForce_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();
        AddDefaultFloatValue("ForcePower", 100f);
        AddDefaultFloatValue("ExplosionRadius", 0.5f);
    }

    public override IEnumerator Execute(ActionController _controller, ActionData data, GameObject target, Vector3 hitpoint)
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

        AddExplosiveForce(rigidbody, data.GetFloatValue("ForcePower"), data.GetFloatValue("ExplosionRadius"), hitpoint);
        yield break;
    }

    private void AddExplosiveForce(Rigidbody rigidbody, float forcePower, float explosionRadius, Vector3 hitPoint)
    {
        if (rigidbody) rigidbody.AddExplosionForce(forcePower, hitPoint, explosionRadius);
    }

}
