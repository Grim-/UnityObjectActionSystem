namespace Grim.ObjectActionSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;

    [CreateAssetMenu(fileName = "Lerp Rotate GameObject", menuName = scriptObjectPath + "Lerp Rotate GameObject")]
    public class LerpRotation_Action : ObjectAction
    {
        public override void Init()
        {
            base.Init();
            AddDefaultVectorValue("Rotation", Vector3.zero);
            AddDefaultFloatValue("Time", 0.5f);
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



            yield return Rotate(targetTransform, data.GetVectorValue("Rotation"), data.GetFloatValue("Time"));

            yield break;
        }

        private IEnumerator Rotate(Transform transform, Vector3 rotation, float duration)
        {
            Tween myTween = transform.DORotate(transform.eulerAngles + rotation, duration);
            yield return myTween.WaitForCompletion();
        }
    }

}