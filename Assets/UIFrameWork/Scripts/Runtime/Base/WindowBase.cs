using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WindowBase : WindowBehaviour
{
    private List<Button> mAllButtonList = new List<Button>(); //所有button列表
    private List<Toggle> mAllToggleList = new List<Toggle>(); //所有toggle列表
    private List<InputField> mAllInputFieldList = new List<InputField>(); //所有输入框列表

    private CanvasGroup mUIMask;
    private CanvasGroup mCanvasGroup;
    protected Transform mUIContent;

    protected bool mDisibleAnim = false; // 是否禁用动画

    /// <summary>
    /// 初始化基类组件
    /// </summary>
    private void InitalizeBaseComponent()
    {
        mUIMask = transform.Find("UIMask").GetComponent<CanvasGroup>();
        mUIContent = transform.Find("UIContent").transform;

        mCanvasGroup = transform.GetComponent<CanvasGroup>();
    }
    
    #region 动画管理

    public void ShowAnimation()
    {
        if(mDisibleAnim) return;
        // if(Canvas.sortingOrder < 100) return; //基础界面不需要缩放动画
        
        //遮罩
        mUIMask.alpha = 0;
        mUIMask.DOFade(1, 0.2f);
        //缩放动画
        mUIContent.localScale = Vector3.one * 0.8f;
        mUIContent.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    public void HideAnimation()
    {
        if (mDisibleAnim)
        {
            UIModule.Instance.HideWindow(Name);
            return;
        }

        // if(Canvas.sortingOrder < 100) return; //基础界面不需要缩放动画
        mUIContent.DOScale(0, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            UIModule.Instance.HideWindow(Name);
        });
    }
    #endregion
    
    #region 生命周期函数
    public override void OnAwake()
    {
        base.OnAwake();
        InitalizeBaseComponent();
    }

    public override void OnShow()
    {
        base.OnShow();
        ShowAnimation();
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

    public void HideWindow()
    {
        // UIModule.Instance.HideWindow(Name);
        HideAnimation();
    }
    public override void SetVisible(bool isVisible)
    {
        // gameobject.SetActive(isVisible); //临时代码
        mCanvasGroup.alpha = isVisible ? 1 : 0;
        mCanvasGroup.blocksRaycasts = isVisible;
        Visible = isVisible;
    }

    public void SetMaskVisible(bool isVisible)
    {
        if (!UISetting.Instance.SINGMASK_SYSTEM)
        {
            return;
        }

        mUIMask.alpha = isVisible ? 1 : 0;
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
