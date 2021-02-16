namespace Grim.ObjectActionSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Disable GameObject", menuName = scriptObjectPath + "Disable GameObject")]
    public class DisableGameObject_Action : ObjectAction
    {
        public override void Init()
        {
            base.Init();
            SetDescription("Disable self or target GameObject");
        }

        public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
        {
            GameObject targetObject = null;

            switch (data.targetType)
            {
                case ActionData.GameObjectActionTarget.SELF:
                    targetObject = _controller.gameObject;
                    break;
                case ActionData.GameObjectActionTarget.TARGET:
                    targetObject = target;
                    break;
            }

            DisableGameObject(targetObject);

            yield break;
        }

        private void DisableGameObject(GameObject gameObject)
        {
            if (gameObject) gameObject.SetActive(false);
        }
    }

}