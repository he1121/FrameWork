/*-----------------------------------
 *Title: UI表现层代码生成工具
 *Author: hed
 *Date:2025/4/17 23:01:05
 *Description:UI 表现层，该层只负责界面交互、UI刷新，不允许编写任何业务逻辑代码。
 *注意：以下文件为自动生成，再次生成不会覆盖原有代码，会在原有代码上进行新增，可放心使用
-----------------------------------*/
using UnityEngine;
using UnityEngine.UI;
using UIFramework;

	 public class TempWindow : WindowBase
	 {
	
		 public TempWindowUIComponent uiCompt = new TempWindowUIComponent();
	
		 #region 生命周期函数
		 //调用机制与Mono Awake一致
		 public override void OnAwake()
		 {
			 uiCompt.InitComponent(this);
			 base.OnAwake();
		 }
		 //物体显示时执行
		 public override void OnShow()
		 {
			 base.OnShow();
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
		 public void OnCloseButtonClick()
		 {
			 HideWindow();
		 }
		 #endregion
	 }
