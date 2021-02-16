namespace Grim.ObjectActionSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Wait for Time", menuName = scriptObjectPath + "Wait for Time")]
    public class WaitForTime_Action : ObjectAction
    {
        public override void Init()
        {
            base.Init();
            SetDescription("Wait for Time (seconds)");
            AddDefaultFloatValue("Wait", 1f);
        }

        public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
        {
            yield return new WaitForSeconds(data.GetFloatValue("Wait"));
        }
    }

}