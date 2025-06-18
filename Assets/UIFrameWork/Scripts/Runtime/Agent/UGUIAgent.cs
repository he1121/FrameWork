using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public static class UGUIAgent
{
    /// <summary>
    /// 指定gameObject物体的SetVisible方法为一下逻辑
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="isVisible"></param>
    public static void SetVisible(this GameObject obj, bool isVisible)
    {
        if (obj == null) return;
        obj.transform.localScale = isVisible ? Vector3.one : Vector3.zero;
    }
    
    public static void SetVisible(this Transform trans, bool isVisible)
    {
        trans.localScale = isVisible ? Vector3.one : Vector3.zero;
    }
    
    public static void SetVisible(this Text text, bool isVisible)
    {
        text.transform.localScale = isVisible ? Vector3.one : Vector3.zero;
    }
    
    public static void SetVisible(this Button btn, bool isVisible)
    {
        btn.transform.localScale = isVisible ? Vector3.one : Vector3.zero;
    }
    
    public static void SetVisible(this Slider slider, bool isVisible)
    {
        slider.transform.localScale = isVisible ? Vector3.one : Vector3.zero;
    }
    
    public static void SetVisible(this Toggle toggle, bool isVisible)
    {
        toggle.transform.localScale = isVisible ? Vector3.one : Vector3.zero;
    }
    
    public static void SetVisible(this InputField inputField, bool isVisible)
    {
        inputField.transform.localScale = isVisible ? Vector3.one : Vector3.zero;
    }
    
    public static void SetVisible(this RawImage rawImage, bool isVisible)
    {
        rawImage.transform.localScale = isVisible ? Vector3.one : Vector3.zero;
    }
    
    public static void SetVisible(this ScrollRect scrollview, bool isVisible)
    {
        scrollview.transform.localScale = isVisible ? Vector3.one : Vector3.zero;
    }
}
