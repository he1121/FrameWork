/*-----------------------------------
 *Title: UI自动化组件生成代码生成工具
 *Author: hed
 *Date:2025/5/20 22:58:30
 *Description:变量需要以[Text]中括号加组件类型的格式声明，然后右键点击窗口物体-一键生成UI数据组件脚本即可
 *注意：以下文件为自动生成，任何手动修改都会被下次生成覆盖，手动修改后尽量避免再次自动生成！
-----------------------------------*/
using UnityEngine;
using UnityEngine.UI;

namespace UIFramework
{
	 public class LoginWindowDataComponent : MonoBehaviour
	{
		public RawImage RawImageRawImage;

		public Button Buttonlogin;

		public Text Texttitle;

		public void InitComponent(WindowBase target)
		{
			//组件查找
	
			 //绑定组件事件
			 LoginWindow mWindow = (LoginWindow)target;
			  target.AddButtonClickListener(Buttonlogin,mWindow.OnloginButtonClick);
		}
	}
}
