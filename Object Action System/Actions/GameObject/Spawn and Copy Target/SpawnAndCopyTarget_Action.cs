using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn and Copy Target", menuName = scriptObjectPath + "Spawn and Copy Target")]
public class SpawnAndCopyTarget_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();
        AddDefaultVectorValue("SpawnOffset", Vector3.zero);
    }

    public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        GameObject spawnPrefab = target;
        Vector3 spawnPosition = Vector3.zero;

        switch (data.targetType)
        {
            case ActionData.GameObjectActionTarget.SELF:
                spawnPosition = new Vector3(hitpoint.x, hitpoint.y, hitpoint.z);
                break;
            case ActionData.GameObjectActionTarget.TARGET:
                spawnPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);

                break;
        }

        spawnPosition = spawnPosition + data.GetVectorValue("SpawnOffset");

        Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);
        yield return null;
    }
}
