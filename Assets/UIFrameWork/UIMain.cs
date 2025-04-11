using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    private void Awake()
    {
        UIModule.Instance.Initalize();
    }

    void Start()
    {
        LoginWindow loginWindow = UIModule.Instance.PopUpWindow<LoginWindow>();
        loginWindow.Test();

        LoginWindow lw = UIModule.Instance.GetWindow<LoginWindow>();
        lw.Test2();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UIModule.Instance.DestrouAllWindow();
        } 
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            UIModule.Instance.DestrouAllWindow(new List<string>{"LoginWindow"});
        } 
    }
}
