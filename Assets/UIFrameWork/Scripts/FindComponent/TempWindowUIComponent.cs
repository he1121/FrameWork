/*-----------------------------------
 *Title: UI自动化组件查找代码生成工具
 *Author: hed
 *Date:2025/4/16 22:38:57
 *注意：以下文件为自动生成，任何手动修改都会被下次生成覆盖，手动修改后尽量避免再次自动生成！
-----------------------------------*/
using UnityEngine;
using UnityEngine.UI;

namespace UIFramework
{
	 public class TempWindowUIComponent
	{
		public Button CloseButton;

		public InputField inputInputField;

		public Toggle togToggle;

		public void InitComponent(WindowBase target)
		{
			//组件查找
			 ButtonClose = (Button)target.transform.GetComponent<Button>();
			 InputFieldinput = (InputField)target.transform.GetComponent<InputField>();
			 Toggletog = (Toggle)target.transform.GetComponent<Toggle>();
	
			 //绑定组件事件
			 TempWindow mWindow = (TempWindow)target;
			  target.AddButtonClickListener(CloseButton,mWindow.OnCloseButtonClick);
			  target.AddInputFieldClickListener(inputInputField,mWindow.OninputInputFieldChange, mWindow.OninputInputFieldEnd);
			  target.AddToggleClickListener(togToggle,mWindow.OntogToggleChange);
		}
	}
}
