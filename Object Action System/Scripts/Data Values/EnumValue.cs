using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnumValue : ActionDataValue
{
    public string name;
    public ForceMode value;

    public EnumValue(string name, ForceMode value)
    {
        this.name = name;
        this.value = value;
    }
}