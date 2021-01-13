using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurveValue : ActionDataValue
{
    public string name;
    public AnimationCurve value;

    public CurveValue(string name, AnimationCurve value)
    {
        this.name = name ?? throw new ArgumentNullException(nameof(name));
        this.value = value ?? throw new ArgumentNullException(nameof(value));
    }
}
