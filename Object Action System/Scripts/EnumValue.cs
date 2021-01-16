using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnumValue : ActionDataValue
{
    public string name;
    public string value;

    public EnumValue(string name, string value)
    {
        this.name = name;
        this.value = value;
    }
}
