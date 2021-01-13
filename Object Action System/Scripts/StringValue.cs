using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StringValue : ActionDataValue
{
    public string name;
    public string value;

    public StringValue(string name, string value)
    {
        this.name = name ?? throw new ArgumentNullException(nameof(name));
        this.value = value ?? throw new ArgumentNullException(nameof(value));
    }
}
