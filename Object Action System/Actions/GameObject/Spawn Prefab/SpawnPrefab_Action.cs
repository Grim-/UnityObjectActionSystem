using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Spawn Prefab", menuName = scriptObjectPath + "Spawn Prefab")]
public class SpawnPrefab_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();

        AddDefaultPrefabValue("SpawnPrefab", null);
        AddDefaultVectorValue("SpawnOffset", Vector3.zero);
    }

    public override IEnumerator Execute(ActionController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        GameObject spawnPrefab = data.GetPrefabValue("SpawnPrefab");
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