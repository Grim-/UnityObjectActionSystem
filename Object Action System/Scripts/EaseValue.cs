using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class EaseValue : ActionDataValue
{
    public string name;
    public Ease value;

    public EaseValue(string name, Ease value)
    {
        this.name = name;
        this.value = value;
    }
}
