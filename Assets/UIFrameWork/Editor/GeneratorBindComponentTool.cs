using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class GeneratorBindComponentTool : Editor
{
    public static List<EditorObjectData> objDataList; //查找对象的数据
    [MenuItem("GameObject/生成组件数据脚本", false, 0)]
    static void CreateFindComponentScript()
    {
        GameObject obj = Selection.objects.First() as GameObject; //获取当前选择的物体
        if (obj == null)
        {
            Debug.LogError("需选择 GameObject");
            return;
        }
        
        objDataList = new List<EditorObjectData>();
        
        //设置脚本生成路径
        if (!Directory.Exists(GeneratorConfig.BindComponentGeneratorPath))
        {
            Directory.CreateDirectory(GeneratorConfig.BindComponentGeneratorPath);
        }
        
        //解析窗口组件数据
        if (GeneratorConfig.ParseType == ParseType.Tag)
        {
            ParseWindowNodeDataByTag(obj.transform, obj.name);
        }
        else
        {
            PresWindowNodeData(obj.transform, obj.name);
        }
        
        //储存字段名称
        string dataListJson = JsonConvert.SerializeObject(objDataList);
        PlayerPrefs.SetString(GeneratorConfig.OBJDATALIST_KEY, dataListJson);
        
        string script = CreateCS(obj.name);
        string scriptPath = GeneratorConfig.BindComponentGeneratorPath + "/" + obj.name + "DataComponent.cs";
        UIWindowEditor.ShowWindow(script, scriptPath);
        
        EditorPrefs.SetString("GeneratorClassName", obj.name + "DataComponent");
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
            }
            PresWindowNodeData(trans.GetChild(i), winName);
        }
    }

    /// <summary>
    /// 通过tag获取当前窗口下指定格式名的组件路径
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="winName"></param>
    public static void ParseWindowNodeDataByTag(Transform trans, string winName)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            GameObject obj = trans.GetChild(i).gameObject;
            string tag = obj.tag;
            if (GeneratorConfig.TAGArr.Contains(tag))
            {
                string fieldName = obj.name;
                string fieldType = tag;
                objDataList.Add(new EditorObjectData{instanceID = obj.GetInstanceID(), fieldName = fieldName, fieldType = fieldType});
            }
            ParseWindowNodeDataByTag(trans.GetChild(i), winName);
        } 
    }
    public static string CreateCS(string name)
    {
        StringBuilder sb = new StringBuilder();
        string nameSpace = "UIFramework";
        sb.AppendLine("/*-----------------------------------");
        sb.AppendLine(" *Title: UI自动化组件生成代码生成工具");
        sb.AppendLine(" *Author: hed");
        sb.AppendLine(" *Date:"+System.DateTime.Now);
        sb.AppendLine(" *Description:变量需要以[Text]中括号加组件类型的格式声明，然后右键点击窗口物体-一键生成UI数据组件脚本即可");
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
        
        sb.AppendLine($"\t public class {name + "DataComponent : MonoBehaviour"}");
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

    //标记AddComponent2Window方法，使其在脚本重新编译后自动调用
    [UnityEditor.Callbacks.DidReloadScripts]
    
    //自动化挂载组件
    public static void AddComponent2Window()
    {
        string className = EditorPrefs.GetString("GeneratorClassName");
        if (string.IsNullOrEmpty(className))
        {
            return;
        }
        
        //1.通过反射的方式从程序集中找到这个脚本，并挂载到当前的物体上
        //获取所有程序集
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        //找到CSharp程序集
        var cSharpAssembly = assemblies.First(assembly => assembly.GetName().Name == "Assembly-CSharp");
        //获取类所在的程序集路径
        string relClassName = "UIFramework." + className;
        Type type = cSharpAssembly.GetType(relClassName);
        if (type == null)
        {
            return;
        }
        
        //获取要挂载的物体
        string windowObjName = className.Replace("DataComponent", "");
        GameObject windowObj = GameObject.Find(windowObjName);
        if (windowObj == null)
        {
            windowObj = GameObject.Find("UIRoot/" + windowObjName);
            if (windowObj == null)
            {
                return;
            }
        }

        //获取窗口上挂载的数据组件 没有则挂载
        Component compt = windowObj.GetComponent(type);
        if (compt == null)
        {
            compt = windowObj.AddComponent(type);
        }
        //2.通过反射的方式，遍历数据列表，找到对应的字段并赋值
        string dataListJson = PlayerPrefs.GetString(GeneratorConfig.OBJDATALIST_KEY);
        List <EditorObjectData> objDataList = JsonConvert.DeserializeObject<List<EditorObjectData>>(dataListJson);
        
        //获取脚本所有字段
        FieldInfo[] fieldInfoList = type.GetFields();
        foreach (var field in fieldInfoList)
        {
            foreach (var objData in objDataList)
            {
                if (field.Name == objData.fieldType + objData.fieldName)
                {
                    GameObject uiObject = EditorUtility.InstanceIDToObject(objData.instanceID) as GameObject;
                    if (string.Equals(objData.fieldType, "GameObject"))
                    {
                        field.SetValue(compt, uiObject);
                    }
                    else
                    {
                        field.SetValue(compt, uiObject.GetComponent(objData.fieldType));
                    }
                    break;
                }
            }
        }
        EditorPrefs.DeleteKey("GeneratorClassName");
    }
}
