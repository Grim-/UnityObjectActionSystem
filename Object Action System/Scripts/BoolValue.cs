using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoolValue : ActionDataValue
{
    public string name;
    public bool value;

    public BoolValue(string name, bool value)
    {
        this.name = name ?? throw new ArgumentNullException(nameof(name));
        this.value = value;
    }
}
