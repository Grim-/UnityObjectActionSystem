namespace Grim.ObjectActionSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Lerp ShaderGraph float Value", menuName = scriptObjectPath + "Lerp ShaderGraph Float Value")]
    public class LerpShadergraphFloatValue : ObjectAction
    {
        public override void Init()
        {
            base.Init();
            SetDescription("Lerp a ShaderGraph float Value by PropertyName to Value over LerpTime");
            AddDefaultStringValue("PropertyName", "DissolveValue");
            AddDefaultFloatValue("Value", 0f);
            AddDefaultFloatValue("LerpTime", 1f);
        }

        public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
        {
            Renderer targetObject = null;

            switch (data.targetType)
            {
                case ActionData.GameObjectActionTarget.SELF:
                    targetObject = _controller.GetComponent<Renderer>();
                    break;
                case ActionData.GameObjectActionTarget.TARGET:
                    targetObject = target.GetComponent<Renderer>();
                    break;
            }

            yield return LerpFloatValue(targetObject, data.GetStringValue("PropertyName"), data.GetFloatValue("Value"), data.GetFloatValue("LerpTime"));


            yield break;
        }


        private IEnumerator LerpFloatValue(Renderer targetObject, string propertyName, float value, float lerpTime)
        {
            float timer = 0;
            float currentValue = targetObject.material.GetFloat(propertyName);

            while (timer < lerpTime)
            {
                timer += Time.deltaTime;

                targetObject.material.SetFloat(propertyName, Mathf.Lerp(currentValue, value, timer / lerpTime));

                yield return null;
            }

            timer = 0f;

            yield break;
        }
    }

}