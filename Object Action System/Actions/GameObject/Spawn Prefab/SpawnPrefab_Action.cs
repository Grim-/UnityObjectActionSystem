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
        AddDefaultBoolValue("ParentToTarget", false);
    }

    public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        GameObject targetObject = null;

        GameObject spawnPrefab = data.GetPrefabValue("SpawnPrefab");
        Vector3 spawnPosition = Vector3.zero;

        switch (data.targetType)
        {
            case ActionData.GameObjectActionTarget.SELF:
                targetObject = _controller.gameObject;
                spawnPosition = new Vector3(targetObject.transform.position.x, targetObject.transform.position.y, targetObject.transform.position.z);
                break;
            case ActionData.GameObjectActionTarget.TARGET:
                targetObject = target;
                spawnPosition = new Vector3(target.transform.position.x /2, target.transform.position.y /2, target.transform.position.z /2);

                break;
        }

        spawnPosition = spawnPosition + data.GetVectorValue("SpawnOffset");

        if(data.GetBoolValue("ParentToTarget") && targetObject)
        {
            Instantiate(spawnPrefab, spawnPosition, Quaternion.identity, targetObject.transform);
        }
        else
        {
            Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);
        }
    
        yield return null;
    }


}