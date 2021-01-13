using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Random Prefab", menuName = scriptObjectPath + "Spawn Random Prefab")]
public class SpawnRandomPrefab_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();
        AddDefaultPrefabValue("Prefab", null);
        AddDefaultVectorValue("SpawnOffset", Vector3.zero);
    }

    public override IEnumerator Execute(ActionController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        List<GameObject> list = data.GetAllPrefabValues();

        int count = list.Count;
        int random = Random.Range(0, count);

        Vector3 spawnPosition = Vector3.zero;

        switch (data.targetType)
        {
            case ActionData.GameObjectActionTarget.SELF:
                Bounds colliderBounds = _controller.GetComponent<Collider>().bounds;

                float x = Random.Range(colliderBounds.min.x, colliderBounds.max.x);
                float y = _controller.transform.position.y;
                float z = Random.Range(colliderBounds.min.z, colliderBounds.max.z);
                spawnPosition = new Vector3(x, y, z);
                break;
            case ActionData.GameObjectActionTarget.TARGET:
                Bounds targetColliderBounds = target.GetComponent<Collider>().bounds;

                float Targetx = Random.Range(targetColliderBounds.min.x, targetColliderBounds.max.x);
                float Targety = target.transform.position.y;
                float Targetz = Random.Range(targetColliderBounds.min.z, targetColliderBounds.max.z);
                spawnPosition = new Vector3(Targetx, Targety, Targetz);

                break;
        }
        spawnPosition = spawnPosition + data.GetVectorValue("SpawnOffset");

        if (list[random]) Instantiate(list[random], spawnPosition, Quaternion.identity);       
        yield break;
    }
}
