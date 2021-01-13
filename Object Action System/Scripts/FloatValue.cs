using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatValue : ActionDataValue
{
    public string name;
    public float value;

    public FloatValue(string name, float value)
    {
        this.name = name ?? throw new ArgumentNullException(nameof(name));
        this.value = value;
    }
}
