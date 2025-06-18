using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    private void Awake()
    {
        UIModule.Instance.Initalize();
    }

    void Start()
    {
        UIModule.Instance.PopUpWindow<LoginWindow>();
        UIModule.Instance.PreLoadWindow<PassWindow>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UIModule.Instance.PushAndPopStackWindow<SignupWindow>();
        }
    }
}
