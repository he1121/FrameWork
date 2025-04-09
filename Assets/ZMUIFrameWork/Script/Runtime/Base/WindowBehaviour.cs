using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindowBehaviour
{
    /// <summary>
    /// 当前窗口物体
    /// </summary>
    public GameObject gameobject { get; set; }
    
    public Transform transform { get; set; }
    
    public Canvas Canvas { get; set; }
    
    public string Name{ get; set; }
    
    public bool Visible { get; set; }
    
    public virtual void OnAwake(){}//只在物体创建时执行一次
    
    public virtual void OnShow(){} //物体显示时调用一次 与mono OnEnable一致
    
    public virtual void OnUpdate(){}
    
    public virtual void OnHide(){}//物体显示时调用一次 与mono OnDisable一致
    
    public virtual void OnDestroy(){}
    
    public virtual void SetVisible(bool isVisible){}
}
