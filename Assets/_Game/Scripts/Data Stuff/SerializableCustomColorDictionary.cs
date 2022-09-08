using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableCustomColorDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver where TValue : List<CustomColor>
{
    public List<TKey> KeyList = new List<TKey>();
    public List<int> PreEndIndex = new List<int>();
    public List<CustomColor> ColorList = new List<CustomColor>();
    public void OnBeforeSerialize()
    {
        KeyList.Clear();
        PreEndIndex.Clear();
        ColorList.Clear();

        int curIndex = 0;
        foreach (KeyValuePair<TKey, TValue> item in this)
        {
            KeyList.Add(item.Key);
            curIndex += item.Value.Count;
            PreEndIndex.Add(curIndex);
            foreach (var color in item.Value)
            {
                ColorList.Add(color);
            }
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if (KeyList.Count != PreEndIndex.Count)
        {
            Debug.LogError("Dictionary data serialize error");
        }

        List<CustomColor> tempList = new List<CustomColor>();
        int index = 0;
        for (int i = 0; i < ColorList.Count; i++)
        {
            if (i < PreEndIndex[index])
            {
                tempList.Add(ColorList[i]);
            }
            else
            {
                this.Add(KeyList[index], tempList as TValue);
                tempList = new List<CustomColor>();
                tempList.Add(ColorList[i]);
                index++;
            }
            Debug.LogWarning("ser  " + ColorList[i] + "    " + tempList.Count);
        }
        Debug.LogWarning(this.Count + "Serializable");
        //NOTE: Add last list to dictionary
        this.Add(KeyList[index], tempList as TValue);

        Debug.LogWarning(this.Count + "Serializable");
        //NOTE: add empty list to remain enum
        if (this.Count != KeyList.Count)
        {
            for (int i = index + 1; i < KeyList.Count; i++)
            {
                this.Add(KeyList[i], new List<CustomColor>() as TValue);
            }
        }

        foreach (KeyValuePair<TKey, TValue> item in this)
        {
            Debug.LogWarning(item.Key + "  ser  " + item.Value.Count);
        }
    }
}
