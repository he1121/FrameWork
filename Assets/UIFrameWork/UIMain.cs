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
            UIModule.Instance.PopUpWindow<AccountWindow>();
        } 
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            UIModule.Instance.PopUpWindow<SignupWindow>();
        } 
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            UIModule.Instance.HideWindow<AccountWindow>();
        } 
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            UIModule.Instance.HideWindow<SignupWindow>();
        } 
    }
}
