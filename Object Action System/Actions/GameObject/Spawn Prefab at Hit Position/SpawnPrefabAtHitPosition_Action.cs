namespace Grim.ObjectActionSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    [CreateAssetMenu(fileName = "Spawn Prefab at event HitPosition", menuName = scriptObjectPath + "Spawn Prefab at event HitPosition")]
    public class SpawnPrefabAtHitPosition_Action : ObjectAction
    {
        public override void Init()
        {
            base.Init();
            SetDescription("Spawn the PrefabValue at the world location where the event was triggered.");
            AddDefaultPrefabValue("SpawnPrefab", null);
        }

        public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
        {
            GameObject spawnPrefab = data.GetPrefabValue("SpawnPrefab");
            Vector3 spawnPosition = Vector3.zero;

            Instantiate(spawnPrefab, hitpoint, Quaternion.identity);
            yield break;
        }

    }

}