namespace Grim.ObjectActionSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Move GameObject", menuName = scriptObjectPath + "Move GameObject")]
    public class TranslatePosition_Action : ObjectAction
    {
        public override void Init()
        {
            base.Init();
            SetDescription("Translates transform position to MoveVector");
            AddDefaultVectorValue("MoveVector", Vector3.zero);
        }

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

            Translate(targetTransform, data.GetVectorValue("MoveVector"));
            yield break;
        }

        private void Translate(Transform transform, Vector3 moveVector)
        {
            if (transform) transform.Translate(moveVector);
        }

    }

}