using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grim.ObjectActionSystem
{
    [CreateAssetMenu(fileName = "Lerp Position", menuName = scriptObjectPath + "Lerp Position")]
    public class LerpPosition_Action : ObjectAction
    {
        public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
        {
            Transform transform = null;

            switch (data.targetType)
            {
                case ActionData.GameObjectActionTarget.SELF:
                    transform = _controller.transform;
                    break;
                case ActionData.GameObjectActionTarget.TARGET:
                    transform = target.transform;
                    break;
            }


            if (data.GetBoolValue("WaitForComplete"))
            {
                yield return transform.DOMove(transform.position + data.GetVectorValue("LerpPosition"), data.GetFloatValue("LerpTime")).SetEase(Ease.Linear).WaitForCompletion();

            }
            else
            {
                yield return transform.DOMove(transform.position + data.GetVectorValue("LerpPosition"), data.GetFloatValue("LerpTime")).SetEase(Ease.Linear);

            }



            yield break;
        }

        public override void Init()
        {
            base.Init();
            AddDefaultVectorValue("LerpPosition", Vector3.zero);
            AddDefaultFloatValue("LerpTime", 1);
            AddDefaultBoolValue("WaitForComplete", true);
            SetDescription("Lerps Target Position by LerPosition over LerpTime");
        }
    }

}