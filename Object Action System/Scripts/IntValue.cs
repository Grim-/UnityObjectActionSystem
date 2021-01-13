using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntValue : ActionDataValue
{
    public string name;
    public int value;

    public IntValue(string name, int value)
    {
        this.name = name ?? throw new ArgumentNullException(nameof(name));
        this.value = value;
    }
}
