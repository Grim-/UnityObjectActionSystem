using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//GEt Values from here
[System.Serializable]
public abstract class ObjectAction : ScriptableObject
{
    public const string scriptObjectPath = "Scriptable Objects/Object Action System/Object Actions/";

    public List<StringValue> stringValues = new List<StringValue>();
    public List<FloatValue> floatValues = new List<FloatValue>();
    public List<PrefabValue> prefabValues  = new List<PrefabValue>();
    public List<IntValue> intValues = new List<IntValue>();
    public List<Vector3Value> vectorValues = new List<Vector3Value>();
    public List<ActionValue> actionValues = new List<ActionValue>();
    public List<BoolValue> boolValues = new List<BoolValue>();
    public List<EnumValue> enumValues = new List<EnumValue>();
    public List<ScriptableObjectValue> soValues = new List<ScriptableObjectValue>();

    public abstract IEnumerator Execute(BaseController _controller, ActionData data, GameObject target, Vector3 hitpoint);

    private void OnEnable()
    {
        Init();
    }

    public virtual void Init()
    {
        
    }

    public void AddDefaultEnumOfType(string name, string value)
    {
        EnumValue newFloat = new EnumValue(name, value);
        if (enumValues.Find(x => x.name == name) == null) enumValues.Add(newFloat);
    }

    public void AddDefaultFloatValue(string name, float value)
    {
        FloatValue newFloat = new FloatValue(name, value);
        if (floatValues.Find(x=> x.name == name) == null) floatValues.Add(newFloat);      
    }

    public void AddDefaultStringValue(string name, string value)
    {
        StringValue newString = new StringValue(name, value);
        if (stringValues.Find(x => x.name == name) == null) stringValues.Add(newString);
    }

    public void AddDefaultBoolValue(string name, bool value)
    {
        BoolValue newBool = new BoolValue(name, value);
        if (boolValues.Find(x => x.name == name) == null) boolValues.Add(newBool);
    }

    public void AddDefaultPrefabValue(string name, GameObject value)
    {
        PrefabValue newValue = new PrefabValue(name, value);
        if (prefabValues.Find(x => x.name == name) == null) prefabValues.Add(newValue);       
    }

    public void AddDefaultSOValue(string name, ScriptableObject value)
    {
        ScriptableObjectValue newValue = new ScriptableObjectValue(name, value);
        if (soValues.Find(x => x.name == name) == null) soValues.Add(newValue);
    }

    public void AddDefaultIntValue(string name, int value)
    {
        IntValue newValue = new IntValue(name, value);
        if (intValues.Find(x => x.name == name) == null) intValues.Add(newValue);       
    }

    public void AddDefaultVectorValue(string name, Vector3 value)
    {
        Vector3Value newValue = new Vector3Value(name, value);
        if (vectorValues.Find(x => x.name == name) == null) vectorValues.Add(newValue);       
    }

    public virtual ObjectAction Clone()
    {
        return Object.Instantiate(this);
    }
}