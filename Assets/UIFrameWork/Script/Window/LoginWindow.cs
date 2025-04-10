using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginWindow : WindowBase
{
    public override void OnAwake()
    {
        base.OnAwake();
        Debug.Log("LoginWindow OnAwake");
    }

    public override void OnShow()
    {
        base.OnShow();
        Debug.Log("LoginWindow OnShow");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Debug.Log("LoginWindow OnUpdate");
    }

    public override void OnHide()
    {
        base.OnHide();
        Debug.Log("LoginWindow OnHide");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Debug.Log("LoginWindow OnDestroy");
    }

    public override void SetVisible(bool isVisible)
    {
        base.SetVisible(isVisible);
        Debug.Log("LoginWindow SetVisible " + isVisible);
    }

    public void Test()
    {
        Debug.Log("login window test");
    }

    public void Test2()
    {
        Debug.Log("login window test2=========");
    }
}
