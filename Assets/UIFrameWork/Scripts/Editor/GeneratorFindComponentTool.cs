using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class GeneratorFindComponentTool:Editor
{

    public static Dictionary<int, string> objFindPathDic; // k 物体的instance id， v 物体的查找路径
    public static List<EditorObjectData> objDataList; //查找对象的数据
    [MenuItem("GameObject/生成组件查找脚本", false, 0)]
    static void CreateFindComponentScript()
    {
        GameObject obj = Selection.objects.First() as GameObject; //获取当前选择的物体
        if (obj == null)
        {
            Debug.LogError("需选择 GameObject");
            return;
        }
        
        objFindPathDic = new Dictionary<int, string>();
        objDataList = new List<EditorObjectData>();
        
        //设置脚本生成路径
        if (!Directory.Exists(GeneratorConfig.FindComponentGeneratorPath))
        {
            Directory.CreateDirectory(GeneratorConfig.FindComponentGeneratorPath);
        }
        
        PresWindowNodeData(obj.transform, obj.name);
        
        //储存字段名称
        string dataListJson = JsonConvert.SerializeObject(objDataList);
        PlayerPrefs.SetString(GeneratorConfig.OBJDATALIST_KEY, dataListJson);
        
        string script = CreateCS(obj.name);
        string scriptPath = GeneratorConfig.FindComponentGeneratorPath + "/" + obj.name + "UIComponent.cs";
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
    /// 获取当前窗口下指定格式名的组件路径
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="winName"></param>
    public static void PresWindowNodeData(Transform trans, string winName)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            GameObject obj = trans.GetChild(i).gameObject;
            string name = obj.name;
            if (name.Contains("[") && name.Contains("]"))
            {
                int index = name.IndexOf("]") + 1;
                string fieldType = name.Substring(1, index - 2);
                string fieldName = name.Substring(index, name.Length - index);
                objDataList.Add(new EditorObjectData{instanceID = obj.GetInstanceID(), fieldName = fieldName, fieldType = fieldType});
                
                //计算该节点的查找路径
                string objParh = name;
                bool isFindOver = false;
                Transform parent = obj.transform;
                for(int k = 0; k <= 20; k++)
                {
                    for (int j = 0; j <= k; j++)
                    {
                        if (k == j)
                        {
                            parent = parent.parent;
                            //如果父节点是当前窗口，说明查找结束
                            if (string.Equals(parent.name, winName))
                            {
                                isFindOver = true;
                                break;
                            }
                            else
                            {
                                objParh = objParh.Insert(0, parent.name + "/");
                            }
                        }
                    }
                    
                    if(isFindOver)
                        break;
                }
                objFindPathDic.Add(obj.GetInstanceID(), objParh);
            }
            PresWindowNodeData(trans.GetChild(i), winName);
        }
    }

    public static string CreateCS(string name)
    {
        StringBuilder sb = new StringBuilder();
        string nameSpace = "UIFramework";
        sb.AppendLine("/*-----------------------------------");
        sb.AppendLine(" *Title: UI自动化组件查找代码生成工具");
        sb.AppendLine(" *Author: hed");
        sb.AppendLine(" *Date:"+System.DateTime.Now);
        sb.AppendLine(" *Description:变量需要以[Text]中括号加组件类型的格式声明，然后右键点击窗口物体-一键生成UI组件查找脚本即可");
        sb.AppendLine(" *注意：以下文件为自动生成，任何手动修改都会被下次生成覆盖，手动修改后尽量避免再次自动生成！");
        sb.AppendLine("-----------------------------------*/");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("using UnityEngine.UI;");

        sb.AppendLine();
        
        //生成命名空间
        if(!string.IsNullOrEmpty(nameSpace))
        {
            sb.AppendLine($"namespace {nameSpace}");
            sb.AppendLine("{");
        }
        
        sb.AppendLine($"\t public class {name + "UIComponent"}");
        sb.AppendLine("\t{");
        
        //根据字段数据列表声明字段
        foreach (var item in objDataList)
        {
            sb.AppendLine("\t\t" + "public " + item.fieldType + " " + item.fieldType + item.fieldName + ";\n");
        }
        
        //声明初始化组件接口
        sb.AppendLine("\t\tpublic void InitComponent(WindowBase target)");
        sb.AppendLine("\t\t{");
        sb.AppendLine("\t\t\t//组件查找");
        foreach (var item in objFindPathDic)
        {
            EditorObjectData data = GetEditorObjectData(item.Key);
            string relFieldName = data.fieldType + data.fieldName;
            if (string.Equals("GameObject", data.fieldType))
            {
                sb.AppendLine($"\t\t\t {relFieldName} = target.transform.Find(\"{item.Value}\").gameObject;");
            }
            else if (string.Equals("Transform", data.fieldType))
            {
                sb.AppendLine($"\t\t\t {relFieldName} = target.transform.Find(\"{item.Value}\").transform;");
            }
            else
            {
                sb.AppendLine($"\t\t\t {relFieldName} = target.transform.Find(\"{item.Value}\").GetComponent<{data.fieldType}>();");
            }
        }

        sb.AppendLine("\t");
        sb.AppendLine("\t\t\t //绑定组件事件");
        //得到逻辑类 Window Base => LoginWindow
        sb.AppendLine($"\t\t\t {name} mWindow = ({name})target;");
        
        //生成UI事件绑定代码
        foreach (var item in objDataList)
        {
            string type = item.fieldType;
            string methodName = item.fieldName;
            string suffix = "";
            if (type.Contains("Button"))
            {
                suffix = "Click";
                sb.AppendLine(
                    $"\t\t\t  target.AddButtonClickListener({type}{methodName},mWindow.On{methodName}Button{suffix});");
            }
            if (type.Contains("InputField"))
            {
                sb.AppendLine(
                    $"\t\t\t  target.AddInputFieldClickListener({type}{methodName},mWindow.On{methodName}InputChange, mWindow.On{methodName}InputEnd);");
            }
            if (type.Contains("Toggle"))
            {
                suffix = "Change";
                sb.AppendLine(
                    $"\t\t\t  target.AddToggleClickListener({type}{methodName},mWindow.On{methodName}Toggle{suffix});");
            }
        }

        sb.AppendLine("\t\t}");
        sb.AppendLine("\t}");
        
        if(!string.IsNullOrEmpty(nameSpace))
        {
            sb.AppendLine("}");
        }
        
        return sb.ToString();
    }
    
    public static EditorObjectData GetEditorObjectData(int instanceID)
    {
        foreach (var item in objDataList)
        {
            if (item.instanceID == instanceID)
            {
                return item;
            }
        }
        return null;
    }
    

}
