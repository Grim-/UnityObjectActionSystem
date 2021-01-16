using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "Lerp ParticleSystem Start Size", menuName = scriptObjectPath + "Lerp ParticleSystem Start Size")]
public class LerpParticleSystemStartSize_Action : ObjectAction
{
    public override void Init()
    {
        base.Init();
        AddDefaultFloatValue("StartSize", 0f);
        AddDefaultFloatValue("LerpTime", 0f);
    }

    public override IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint)
    {
        ParticleSystem particleSystem = null;

        switch (data.targetType)
        {
            case ActionData.GameObjectActionTarget.SELF:
                particleSystem = _controller.GetComponent<ParticleSystem>();
                break;
            case ActionData.GameObjectActionTarget.TARGET:
                particleSystem = target.GetComponent<ParticleSystem>();
                break;
        }
        yield return LerpStartSize(particleSystem, data.GetFloatValue("StartSize"), data.GetFloatValue("LerpTime"));
        yield break;
    }


    private IEnumerator LerpStartSize(ParticleSystem particleSystem, float value, float lerpTime)
    {
        float timer = 0;
        var main = particleSystem.main;

        float currentValue = main.startSize.constant;

        while (timer < lerpTime)
        {
            timer += Time.deltaTime;

            main.startSize = Mathf.Lerp(currentValue, value, timer / lerpTime);
            yield return null;
        }

        timer = 0f;

        yield break;
    }
}
