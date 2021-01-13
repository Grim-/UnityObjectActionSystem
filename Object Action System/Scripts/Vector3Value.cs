using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector3Value : ActionDataValue
{
    public string name;
    public Vector3 value;

    public Vector3Value(string name, Vector3 value)
    {
        this.name = name;
        this.value = value;
    }
}
