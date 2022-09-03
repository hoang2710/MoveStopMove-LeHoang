using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    public List<TKey> KeyList = new List<TKey>();
    public List<TValue> ValueList = new List<TValue>();

    public void OnBeforeSerialize()
    {
        KeyList.Clear();
        ValueList.Clear();
        foreach (KeyValuePair<TKey, TValue> item in this)
        {
            KeyList.Add(item.Key);
            ValueList.Add(item.Value);
        }
    }
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (KeyList.Count != ValueList.Count)
        {
            Debug.LogError("Dictionary data serialize error");
        }

        for (int i = 0; i < KeyList.Count; i++)
        {
            this.Add(KeyList[i], ValueList[i]);
        }
    }
}
