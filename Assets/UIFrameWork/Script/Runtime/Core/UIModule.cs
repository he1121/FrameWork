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
            SetWindowMaskVisible();
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
                SetWindowMaskVisible();
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
            SetWindowMaskVisible();
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

    private void DestroyWindow(string wndName)
    {
        WindowBase window = GetWindow(wndName);
        DestroyWindow(window);
    }

    private void DestroyWindow(WindowBase window)
    {
        if (window != null)
        {
            if (mAllWindowDisc.ContainsKey(window.Name))
            {
                mAllWindowDisc.Remove(window.Name);
                mAllWindowList.Remove(window);
                mAllVisibleWindowList.Remove(window);
                
                window.SetVisible(false);
                SetWindowMaskVisible();
                window.OnHide();
                window.OnDestroy();
                GameObject.Destroy(window.gameobject.gameObject);
            }
        }
    }

    public void DestroyWindow<T>() where T : WindowBase
    {
        DestroyWindow(typeof(T).Name);
    }

    public void DestrouAllWindow(List<string> filterList =null)
    {
        for (int i = mAllWindowList.Count - 1; i >= 0; i--)
        {
            WindowBase window = mAllWindowList[i];
            if (window == null || (filterList != null && filterList.Contains(window.Name)))
            {
                continue;
            }
            DestroyWindow(window.Name);
            Resources.UnloadUnusedAssets();
        }
    }

    private void SetWindowMaskVisible()
    {
        if (!UISetting.Instance.SINGMASK_SYSTEM)
        {
            return;
        }
        
        //1.关闭所有窗口的mask 设置为不可见
        //2.从所有可见窗口中找到层级最大的窗口设置为可见
        
        WindowBase maxOrderWindowBase = null; //最大渲染层级的窗口
        int maxOrder = 0; // 最大渲染层级
        int maxIndex = 0; //最大排序下标 在相同父节点下的排序下标

        for (int i = 0; i < mAllVisibleWindowList.Count; i++)
        {
            WindowBase window = mAllWindowList[i];
            if (window != null && window.gameobject != null)
            {
                window.SetMaskVisible(false);

                var renderOrder = window.Canvas.renderOrder;
                var curIndex = window.transform.GetSiblingIndex();
                if (maxOrderWindowBase == null)
                {
                    maxOrderWindowBase = window;
                    maxOrder = renderOrder;
                    maxIndex = curIndex;
                }
                else
                {
                    if (maxOrder < renderOrder)
                    {
                        maxOrderWindowBase = window;
                        maxOrder = renderOrder;
                    }
                    else if (maxOrder == renderOrder && maxIndex < curIndex)
                    {
                        maxOrderWindowBase = window;
                        maxIndex = curIndex;
                    }
                }
            }
        }

        if (maxOrderWindowBase != null)
        {
            maxOrderWindowBase.SetMaskVisible(true);
        }
    }

    public GameObject TempLoadWindow(string wndName)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Window/" + wndName), mUIRoot);
        obj.name = wndName;
        obj.transform.localScale = Vector3.one;
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        return obj;
    }
}
