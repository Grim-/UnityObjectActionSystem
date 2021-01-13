using UnityEngine;
using System.Collections;

[System.Serializable]
public class PrefabValue : ActionDataValue
{
    public string name;
    public GameObject value;

    public PrefabValue(string name, GameObject value)
    {
        this.name = name;
        this.value = value;
    }
}
