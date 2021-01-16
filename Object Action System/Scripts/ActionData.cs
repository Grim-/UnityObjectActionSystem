using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NEEDS TO BE COPIED HERE FROM OBJECT ACTION
[System.Serializable]
public class ActionData
{
    public GameObjectActionTarget targetType;
    public List<StringValue> stringValues = new List<StringValue>();
    public List<FloatValue> floatValues = new List<FloatValue>();
    public List<BoolValue> boolValues = new List<BoolValue>();
    public List<PrefabValue> prefabValues = new List<PrefabValue>();
    public List<IntValue> intValues = new List<IntValue>();
    public List<Vector3Value> vectorValues = new List<Vector3Value>();
    public List<ActionValue> actionValues = new List<ActionValue>();
    public List<EnumValue> enumValues = new List<EnumValue>();
    public List<ScriptableObjectValue> soValues = new List<ScriptableObjectValue>();

    public enum GameObjectActionTarget
    {
        SELF,
        TARGET
    }

    public string GetStringValue(string name)
    {
        return stringValues.Find(x => x.name == name).value;
    }

    public ScriptableObject GetSOValue(string name)
    {
        return soValues.Find(x => x.name == name).value;
    }

    public float GetFloatValue(string name)
    {
        return floatValues.Find(x => x.name == name).value;
    }

    public bool GetBoolValue(string name)
    {
        return boolValues.Find(x => x.name == name).value;
    }

    public int GetIntValue(string name)
    {
        return intValues.Find(x => x.name == name).value;
    }

    public Vector3 GetVectorValue(string name)
    {
        return vectorValues.Find(x => x.name == name).value;
    }

    public T GetEnumValue<T>(Type type, string name) where T : Enum
    {
        T myEnum = (T)Enum.Parse(type, enumValues.Find(x => x.name == name).value);
        return myEnum;
    }
    //;
    public List<GameObject> GetAllPrefabValues()
    {
        List<GameObject> prefabs = new List<GameObject>();

        foreach (var prefab in prefabValues)
        {
            prefabs.Add(prefab.value);
        }
        return prefabs;
    }

    public void AddFloatValue(string name, float value)
    {
        FloatValue newFloat = new FloatValue(name, value);
        if (floatValues.Find(x => x.name == name) == null) floatValues.Add(newFloat);
    }

    public void AddStringValue(string name, string value)
    {
        StringValue newString = new StringValue(name, value);
        if (stringValues.Find(x => x.name == name) == null) stringValues.Add(newString);
    }

    public void AddBoolValue(string name, bool value)
    {
        BoolValue newBool = new BoolValue(name, value);
        if (boolValues.Find(x => x.name == name) == null) boolValues.Add(newBool);
    }

    public void AddPrefabValue(string name, GameObject value)
    {
        PrefabValue newValue = new PrefabValue(name, value);
        if (prefabValues.Find(x => x.name == name) == null) prefabValues.Add(newValue);
    }

    public void AddSOValue(string name, ScriptableObject value)
    {
        ScriptableObjectValue newValue = new ScriptableObjectValue(name, value);
        if (soValues.Find(x => x.name == name) == null) soValues.Add(newValue);
    }

    public void AddIntValue(string name, int value)
    {
        IntValue newValue = new IntValue(name, value);
        if (intValues.Find(x => x.name == name) == null) intValues.Add(newValue);
    }

    public void AddVectorValue(string name, Vector3 value)
    {
        Vector3Value newValue = new Vector3Value(name, value);
        if (vectorValues.Find(x => x.name == name) == null) vectorValues.Add(newValue);
    }

    public GameObject GetPrefabValue(string name)
    {
        return prefabValues.Find(x => x.name == name).value;
    }

    public ObjectAction GetActionValue(string name)
    {
        return actionValues.Find(x => x.name == name).value;
    }

    public void ClearAllValues()
    {
        floatValues.Clear();
        intValues.Clear();
        boolValues.Clear();
        prefabValues.Clear();
        vectorValues.Clear();
    }
}

[System.Serializable]
public class InnerAction
{
    public string name;
    public ObjectAction action;
    public ActionData data;
}
