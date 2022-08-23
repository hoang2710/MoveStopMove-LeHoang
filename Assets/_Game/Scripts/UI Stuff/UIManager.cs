using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UICanvasRef
{
    public UICanvasID Id;
    public UICanvas Canvas;
}
public class UIManager : SingletonMono<UIManager>
{
    public Dictionary<UICanvasID, UICanvas> UICanvasReferenceDict = new Dictionary<UICanvasID, UICanvas>();
    [NonReorderable]
    public List<UICanvasRef> UIRefList;
    public Dictionary<UICanvasID, UICanvas> UICanvasDict = new Dictionary<UICanvasID, UICanvas>();
    public Transform UICanvasParentTrans;

    protected override void Awake()
    {
        base.Awake();

        InitCanvasData();
    }
    private void InitCanvasData()
    {
        foreach (var item in UIRefList)
        {
            if (item.Canvas != null)
            {
                UICanvasReferenceDict.Add(item.Id, item.Canvas);
            }
        }
    }
    public UICanvas GetUICanvas(UICanvasID id)
    {
        if (!UICanvasDict.ContainsKey(id) || UICanvasDict[id] == null)
        {
            UICanvas canvas = Instantiate(UICanvasReferenceDict[id], UICanvasParentTrans);
            UICanvasDict[id] = canvas;
        }
        return UICanvasDict[id];
    }
    public T GetUICanvas<T>(UICanvasID id) where T : UICanvas
    {
        return GetUICanvas(id) as T;
    }
    public UICanvas OpenUI(UICanvasID id)
    {
        UICanvas canvas = GetUICanvas(id);

        canvas.Open();

        return canvas;
    }
    public T OpenUI<T>(UICanvasID id) where T : UICanvas
    {
        return OpenUI(id) as T;
    }
    public void CloseUI(UICanvasID id)
    {
        if (IsUICanvasOpened(id))
        {
            GetUICanvas(id).Close();
        }
    }
    public bool IsUICanvasOpened(UICanvasID id)
    {
        return UICanvasDict.ContainsKey(id) && UICanvasDict[id] != null && UICanvasDict[id].gameObject.activeInHierarchy;
    }
}