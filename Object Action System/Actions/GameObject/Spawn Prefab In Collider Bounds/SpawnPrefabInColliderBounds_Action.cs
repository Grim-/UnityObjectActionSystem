namespace Grim.ObjectActionSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Spawn Prefab In Collider Bounds", menuName = scriptObjectPath + "Spawn Prefab In Collider Bounds")]
    public class SpawnPrefabInColliderBounds_Action : ObjectAction
    {
        public override void Init()
        {
            base.Init();
            SetDescription("Spawn the PrefabValue within self or target collider bounds");
            AddDefaultPrefabValue("SpawnPrefab", null);
            AddDefaultVectorValue("SpawnOffset", Vector3.zero);
        }

        public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
        {
            GameObject spawnPrefab = data.GetPrefabValue("SpawnPrefab");
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

            Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);
            yield return null;
        }
    }

}