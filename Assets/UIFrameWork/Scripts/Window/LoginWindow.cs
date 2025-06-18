/*-----------------------------------
 *Title: UI表现层代码生成工具
 *Author: hed
 *Date:2025/5/20 21:13:03
 *Description:UI 表现层，该层只负责界面交互、UI刷新，不允许编写任何业务逻辑代码。
 *注意：以下文件为自动生成，再次生成不会覆盖原有代码，会在原有代码上进行新增，可放心使用
-----------------------------------*/
using UnityEngine;
using UnityEngine.UI;
using UIFramework;

	 public class LoginWindow : WindowBase
	 {
	
		 public LoginWindowDataComponent dataCompt;
	
		 #region 生命周期函数
		 //调用机制与Mono Awake一致
		 public override void OnAwake()
		 {
			 dataCompt = gameobject.GetComponent<LoginWindowDataComponent>();
			 dataCompt.InitComponent(this);
			 base.OnAwake();
		 }
		 //物体显示时执行
		 public override void OnShow()
		 {
			 base.OnShow();
			 
			 // UIModule.Instance.PushWindowToStack<SignupWindow>();
			 // UIModule.Instance.PushWindowToStack<PassWindow>();
			 // UIModule.Instance.PushWindowToStack<SettingWindow>();
			 
			 // UIModule.Instance.StartPopFirstStackWindow();
			 
			 UIModule.Instance.PushAndPopStackWindow<SignupWindow>();
			 UIModule.Instance.PushAndPopStackWindow<PassWindow>();
			 UIModule.Instance.PushAndPopStackWindow<SettingWindow>();
		 }
		 //物体隐藏时执行
		 public override void OnHide()
		 {
			 base.OnHide();
		 }
		 //物体销毁时执行
		 public override void OnDestroy()
		 {
			 base.OnDestroy();
		 }
		 #endregion
		 #region API Function
	   
		 #endregion
	
		 #region UI组件事件
		 public void OnloginButtonClick()
		 {
			 HideWindow();
		 }
		 public void OntitleButtonClick()
		 {
			 
		 }
		 #endregion
	 }
