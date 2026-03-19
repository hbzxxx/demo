using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UIData : ISerializationCallbackReceiver
{
    [NonSerialized]
    public UIType panelType;

    public string panelTypestring;
    public string path;

    //反序列化
    public void OnBeforeSerialize()
    {
    }
    public void OnAfterDeserialize()
    {
        UIType type = (UIType)System.Enum.Parse(typeof(UIType), panelTypestring);
        panelType = type;
    }
}
