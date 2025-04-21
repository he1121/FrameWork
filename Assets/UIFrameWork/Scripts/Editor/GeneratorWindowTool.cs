using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class GeneratorWindowTool : Editor
{
    static Dictionary<string, string> methodDic = new Dictionary<string, string>();

    [MenuItem("GameObject/生成Window脚本", false, 0)]
    static void CreateFindComponentScript()
    {
        GameObject obj = Selection.objects.First() as GameObject; //获取当前选择的物体
        if (obj == null)
        {
            Debug.LogError("需选择 GameObject");
            return;
        }
        
        
        //设置脚本生成路径
        if (!Directory.Exists(GeneratorConfig.WindowGeneratorPath))
        {
            Directory.CreateDirectory(GeneratorConfig.WindowGeneratorPath);
        }
        
        //生成cs脚本
        string script = CreateWindowCs(obj.name);
        string scriptPath = GeneratorConfig.WindowGeneratorPath + "/" + obj.name + ".cs";
        if (File.Exists(scriptPath))
        {
            File.Delete(scriptPath);
        }
        
        StreamWriter writer = File.CreateText(scriptPath);
        writer.Write(script);
        writer.Close();
        
        AssetDatabase.Refresh();
        
        Debug.Log("生成脚本文件" + scriptPath);
        Debug.Log(script);
    }

    /// <summary>
    /// 生成Window脚本
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string CreateWindowCs(string name)
    {
        //储存字段名称
        string dataListJson = PlayerPrefs.GetString(GeneratorConfig.OBJDATALIST_KEY);
        List<EditorObjectData> objDataList = JsonConvert.DeserializeObject<List<EditorObjectData>>(dataListJson);
        methodDic.Clear();
        //生成脚本
        StringBuilder sb = new StringBuilder();
        string nameSpace = "UIFramework";
        sb.AppendLine("/*-----------------------------------");
        sb.AppendLine(" *Title: UI表现层代码生成工具");
        sb.AppendLine(" *Author: hed");
        sb.AppendLine(" *Date:"+System.DateTime.Now);
        sb.AppendLine(" *Description:UI 表现层，该层只负责界面交互、UI刷新，不允许编写任何业务逻辑代码。");
        sb.AppendLine(" *注意：以下文件为自动生成，再次生成不会覆盖原有代码，会在原有代码上进行新增，可放心使用");
        sb.AppendLine("-----------------------------------*/");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("using UnityEngine.UI;");
        sb.AppendLine("using UIFramework;");
        sb.AppendLine();
        
        //生成类名
        sb.AppendLine($"\t public class {name} : WindowBase");
        sb.AppendLine("\t {");
        sb.AppendLine("\t");
        
        //生成字段
        sb.AppendLine($"\t\t public {name}UIComponent uiCompt = new {name}UIComponent();");
        
        //生成生命周期函数 Awake
        sb.AppendLine("\t");
        sb.AppendLine("\t\t #region 生命周期函数");
        sb.AppendLine("\t\t //调用机制与Mono Awake一致");
        sb.AppendLine("\t\t public override void OnAwake()");
        sb.AppendLine("\t\t {");
        sb.AppendLine("\t\t\t uiCompt.InitComponent(this);");
        sb.AppendLine("\t\t\t base.OnAwake();");
        sb.AppendLine("\t\t }");
        
        //生成生命周期函数 OnShow
        sb.AppendLine("\t\t //物体显示时执行");
        sb.AppendLine("\t\t public override void OnShow()");
        sb.AppendLine("\t\t {");
        sb.AppendLine("\t\t\t base.OnShow();");
        sb.AppendLine("\t\t }");
        
        //生成生命周期函数 OnHide
        sb.AppendLine("\t\t //物体隐藏时执行");
        sb.AppendLine("\t\t public override void OnHide()");
        sb.AppendLine("\t\t {");
        sb.AppendLine("\t\t\t base.OnHide();");
        sb.AppendLine("\t\t }");
        
        //生成生命周期函数 OnDestroy
        sb.AppendLine("\t\t //物体销毁时执行");
        sb.AppendLine("\t\t public override void OnDestroy()");
        sb.AppendLine("\t\t {");
        sb.AppendLine("\t\t\t base.OnDestroy();");
        sb.AppendLine("\t\t }");
        
        sb.AppendLine("\t\t #endregion");
        
        //API Function
        sb.AppendLine("\t\t #region API Function");
        sb.AppendLine("\t   ");
        sb.AppendLine("\t\t #endregion");
        
        //UI组件事件生成
        sb.AppendLine("\t");
        sb.AppendLine("\t\t #region UI组件事件");
        foreach (var item in objDataList)
        {
            string type = item.fieldType;
            string methodName = "On"+item.fieldName;
            string suffix = "";
            if (type.Contains("Button"))
            {
                suffix = "ButtonClick";
                CreateMethod(sb, ref methodDic, methodName+suffix);
            }
            else if (type.Contains("InputField"))
            {
                suffix = "InputChange";
                CreateMethod(sb, ref methodDic, methodName+suffix, "string inputText");
                suffix = "InputEnd";
                CreateMethod(sb, ref methodDic, methodName+suffix, "string inputText");
            }
            else if (type.Contains("Toggle"))
            {
                suffix = "ToggleChange";
                CreateMethod(sb, ref methodDic, methodName+suffix, "bool state, Toggle toggle");
            }
        }
        sb.AppendLine("\t\t #endregion");
        sb.AppendLine("\t }");
        return sb.ToString();
    }

    /// <summary>
    /// 生成UI事件方法
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="methodDic"></param>
    /// <param name="methodName"></param>
    /// <param name="param"></param>
    public static void CreateMethod(StringBuilder sb, ref Dictionary<string, string> methodDic, string methodName, string param = "")
    {
        sb.AppendLine($"\t\t public void {methodName}({param})");
        sb.AppendLine("\t\t {");
        if ( methodName == "OnCloseButtonClick")
        {
            sb.AppendLine("\t\t\t HideWindow();");
        }
        sb.AppendLine("\t\t }");
        
        //存储UI组件事件 提供给后续新增代码使用
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"\t\t Public void {methodName}({param})");
        builder.AppendLine("\t\t {");
        builder.AppendLine("\t\t ");
        builder.AppendLine("\t\t }");
        methodDic.Add(methodName, builder.ToString());
    }
}
