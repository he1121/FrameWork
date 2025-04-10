﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WindowBase : WindowBehaviour
{
    //ceshi
    private List<Button> mAllButtonList = new List<Button>(); //所有button列表
    private List<Toggle> mAllToggleList = new List<Toggle>(); //所有toggle列表
    private List<InputField> mAllInputFieldList = new List<InputField>(); //所有输入框列表
    
    #region 生命周期函数
    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void OnShow()
    {
        base.OnShow();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnHide()
    {
        base.OnHide();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        RemoveAllButtonClickListener();
        removeAllToggleClickListener();
        RemoveAllInputFieldClickListener();
        
        mAllButtonList.Clear();
        mAllToggleList.Clear();
        mAllInputFieldList.Clear();
    }

    public override void SetVisible(bool isVisible)
    {
        base.SetVisible(isVisible);
    }

    #endregion

    #region 事件管理

    public void AddButtonClickListener(Button btn, UnityAction action)
    {
        if (btn != null)
        {
            if (!mAllButtonList.Contains(btn))
            {
                mAllButtonList.Add(btn);
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(action);
            }
        }
    }

    public void AddToggleClickListener(Toggle toggle, UnityAction<bool, Toggle> action)
    {
        if (toggle != null)
        {
            if (!mAllToggleList.Contains(toggle))
            {
                mAllToggleList.Add(toggle);
            }
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((isOn) =>
            {
                action?.Invoke(isOn, toggle);
            });
        }
    }

    public void AddInputFieldClickListener(InputField inputField, UnityAction<string> onChangeAction,
        UnityAction<string> onEndAction)
    {
        if (inputField != null)
        {
            if (!mAllInputFieldList.Contains(inputField))
            {
                mAllInputFieldList.Add(inputField);
            }
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onEndEdit.RemoveAllListeners();
            
            inputField.onValueChanged.AddListener(onChangeAction);
            inputField.onEndEdit.AddListener(onEndAction);
        }
    }

    public void RemoveAllButtonClickListener()
    {
        foreach (var btn in mAllButtonList)
        {
            btn.onClick.RemoveAllListeners();
        }
    }
    
    public void removeAllToggleClickListener()
    {
        foreach (var toggle in mAllToggleList)
        {
            toggle.onValueChanged.RemoveAllListeners();
        }
    }

    public void RemoveAllInputFieldClickListener()
    {
        foreach (var input in mAllInputFieldList)
        {
            input.onValueChanged.RemoveAllListeners();
            input.onEndEdit.RemoveAllListeners();
        }
    }
    #endregion
}
