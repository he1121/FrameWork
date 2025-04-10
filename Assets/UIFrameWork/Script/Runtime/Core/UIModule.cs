using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModule
{
    private static UIModule _instance;

    public static UIModule Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIModule();
            }
            return _instance;
        }
    }

    private Camera mUICamera;
    private Transform mUIRoot;
    
    private Dictionary<string, WindowBase> mAllWindowDisc = new Dictionary<string, WindowBase>();//所以窗口字典
    private List<WindowBase> mAllWindowList = new List<WindowBase>(); //所有窗口列表
    private List<WindowBase> mAllVisibleWindowList = new List<WindowBase>(); //所有可见窗口列表
    public void Initalize()
    {
        mUICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        mUIRoot = GameObject.Find("UIRoot").transform;
    }

    /// <summary>
    /// 弹出一个弹窗
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T PopUpWindow<T>() where T: WindowBase, new()
    {
        System.Type type = typeof(T);
        string wndName = type.Name;
        WindowBase window = GetWindow(wndName);
        if (window != null)
        {
            return ShowWindow(wndName) as T;
        }

        T t = new T();
        return InitalizeWindow(t, wndName) as T;
    }

    private WindowBase InitalizeWindow(WindowBase windowBase, string wndName)
    {
        //1.生成窗口预制体
        GameObject newWindow = TempLoadWindow(wndName);
        //2.初始化对应管理类
        if (newWindow != null)
        {
            windowBase.Name = newWindow.name;
            windowBase.gameobject = newWindow;
            windowBase.transform = newWindow.transform;
            windowBase.Canvas = newWindow.GetComponent<Canvas>();
            windowBase.Canvas.worldCamera = mUICamera;
            windowBase.transform.SetAsLastSibling();
        
            windowBase.OnAwake();
            windowBase.SetVisible(true);
            windowBase.OnShow();

            RectTransform rectTransform = newWindow.GetComponent<RectTransform>();
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
        
            mAllWindowDisc.Add(wndName, windowBase);
            mAllWindowList.Add(windowBase);
            mAllVisibleWindowList.Add(windowBase);
            return windowBase;
        }
    
        Debug.LogError("没有加载窗口 "+wndName);
        return null;
    }
    private WindowBase ShowWindow(string wndName)
    {
        WindowBase window = null;
        if (mAllWindowDisc.ContainsKey(wndName))
        {
            window = mAllWindowDisc[wndName];
            if (window.gameobject != null && window.Visible == false)
            {
                mAllVisibleWindowList.Add(window);
                window.transform.SetAsLastSibling();
                window.SetVisible(true);
                window.OnShow();
            }
            return window;
        }
        else
        {
            Debug.LogError(wndName+"窗口不存在，请调用PopUpWindow进行弹出");
        }

        return null;
    }

    private WindowBase GetWindow(string wndName)
    {
        if (mAllWindowDisc.ContainsKey(wndName))
        {
            return mAllWindowDisc[wndName];
        }
        return null;
    }

    /// <summary>
    /// 获取已经弹出的弹窗
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetWindow<T>() where T : WindowBase
    {
        System.Type type = typeof(T);
        foreach (var wnd in mAllVisibleWindowList)
        {
            if (wnd.Name == type.Name)
            {
                return (T)wnd;
            }
        }
        Debug.LogError("未获取到窗口 "+type.Name);
        return null;
    }

    private void HideWindow(string wndName)
    {
        WindowBase window = GetWindow(wndName);
        HideWindow(window);
    }

    private void HideWindow(WindowBase window)
    {
        if (window != null && window.Visible)
        {
            mAllVisibleWindowList.Remove(window);
            window.SetVisible(false);
            window.OnHide();
        }
    }

    /// <summary>
    /// 隐藏弹窗
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void HideWindow<T>() where T : WindowBase
    {
        HideWindow(typeof(T).Name);
    }

    public GameObject TempLoadWindow(string wndName)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Window/" + wndName));
        obj.name = wndName;
        obj.transform.SetParent(mUIRoot);
        obj.transform.localScale = Vector3.one;
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        return obj;
    }
}
