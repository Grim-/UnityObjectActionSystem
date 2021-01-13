using UnityEngine;
using System.Collections;
[System.Serializable]
public class ScriptableObjectValue
{
    public string name;
    public ScriptableObject value;

    public ScriptableObjectValue(string name, ScriptableObject value)
    {
        this.name = name;
        this.value = value;
    }
}