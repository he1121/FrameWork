/*-----------------------------------
 *Title: UI自动化组件查找代码生成工具
 *Author: hed
 *Date:2025/4/17 23:05:20
 *Description:变量需要以[Text]中括号加组件类型的格式声明，然后右键点击窗口物体-一键生成UI组件查找脚本即可
 *注意：以下文件为自动生成，任何手动修改都会被下次生成覆盖，手动修改后尽量避免再次自动生成！
-----------------------------------*/
using UnityEngine;
using UnityEngine.UI;

namespace UIFramework
{
	 public class PassWindowUIComponent
	{
		public Button ButtonClose;

		public InputField InputFieldinput;

		public Toggle Togglepasasword;

		public void InitComponent(WindowBase target)
		{
			//组件查找
			 ButtonClose = (Button)target.transform.Find("UIContent/[Button]Close").GetComponent<Button>();
			 InputFieldinput = (InputField)target.transform.Find("UIContent/[InputField]input").GetComponent<InputField>();
			 Togglepasasword = (Toggle)target.transform.Find("UIContent/[Toggle]pasasword").GetComponent<Toggle>();
	
			 //绑定组件事件
			 PassWindow mWindow = (PassWindow)target;
			  target.AddButtonClickListener(ButtonClose,mWindow.OnCloseButtonClick);
			  target.AddInputFieldClickListener(InputFieldinput,mWindow.OninputInputChange, mWindow.OninputInputEnd);
			  target.AddToggleClickListener(Togglepasasword,mWindow.OnpasaswordToggleChange);
		}
	}
}
