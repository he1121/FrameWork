using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SystemUIEditor : Editor
{
    [InitializeOnLoadMethod] //标记静态方法，使其在编辑器加载时自动执行，它通常用于初始化编辑器工具或设置全局状态
    private static void InitEditor()
    {
        //编辑器层级视图发生变化时，将触发 HandleTextOrImageRaycast 方法
        EditorApplication.hierarchyChanged += HandleTextOrImageRaycast;
        EditorApplication.hierarchyChanged += LoadWindowCamera;
    }

    /// <summary>
    /// 组件创建时自动禁用 Text Image RawImage 的射线检测功能
    /// </summary>
    private static void HandleTextOrImageRaycast()
    {
        GameObject obj = Selection.activeGameObject;
        if (obj == null) return;

        DisableRaycast<Text>(obj);
        DisableRaycast<Image>(obj);
        DisableRaycast<RawImage>(obj);
    }

    private static void DisableRaycast<T>(GameObject obj) where T: Graphic
    {
        if (obj.TryGetComponent<T>(out T component))
        {
            component.raycastTarget = false;
        }
    }

    private static void LoadWindowCamera()
    {
        if (Selection.activeGameObject != null)
        {
            GameObject uiCamera = GameObject.Find("UICamera");
            if (uiCamera != null)
            {
                Camera camera = uiCamera.GetComponent<Camera>();
                if (Selection.activeGameObject.name.Contains("Window"))
                {
                    Canvas canvas = Selection.activeGameObject.GetComponent<Canvas>();
                    if (canvas != null)
                    {
                        canvas.worldCamera = camera;
                    }
                }
            }
        }
    }
}
