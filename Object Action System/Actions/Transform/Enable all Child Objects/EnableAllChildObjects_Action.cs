namespace Grim.ObjectActionSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Enable Child GameObjects", menuName = scriptObjectPath + "Enable Child GameObjects")]
    public class EnableAllChildObjects_Action : ObjectAction
    {
        public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
        {
            Transform targetTransform = null;

            switch (data.targetType)
            {
                case ActionData.GameObjectActionTarget.SELF:
                    targetTransform = _controller.transform;
                    break;
                case ActionData.GameObjectActionTarget.TARGET:
                    targetTransform = target.transform;
                    break;
            }

            EnableChildObjects(targetTransform);
            yield break;
        }


        private void EnableChildObjects(Transform targetTransform)
        {
            for (int i = 0; i < targetTransform.childCount; i++)
            {
                targetTransform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

}