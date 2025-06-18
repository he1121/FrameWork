using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class UIWindowEditor : EditorWindow
{
    private string scriptContent;
    private string filePath;

    private Vector2 scroll = new Vector2();
    
    /// <summary>
    /// 展示代码窗口
    /// </summary>
    /// <param name="content"></param>
    /// <param name="filePath"></param>
    /// <param name="insterDisc">新增代码数据</param>
    public static void ShowWindow(string content, string filePath, Dictionary<string, string> insterDisc = null)
    {
        //创建代码展示窗口
        UIWindowEditor window = (UIWindowEditor)GetWindowWithRect(typeof(UIWindowEditor), new Rect(100, 50, 800, 700), true, "UIWindowEditor");
        window.scriptContent = content;
        window.filePath = filePath;
        
        //处理新增代码
        if (File.Exists(filePath) && insterDisc != null)
        {
            string originScript = File.ReadAllText(filePath);
            foreach (var item in insterDisc)
            {
                if (!originScript.Contains(item.Key))
                {
                    int index = window.GetInsertIndex(content);
                    window.scriptContent = originScript.Insert(index, item.Value);
                }
            }
        }
        
        window.Show();
    }

    public void OnGUI()
    {
        //绘制ScrollView
        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(600), GUILayout.Width(800));
        EditorGUILayout.TextArea(scriptContent);
        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space();
        
        //绘制脚本生成路径
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextArea("脚本生成路径：" + filePath);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        
        //绘制按钮
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("生成脚本", GUILayout.Height(30)))
        {
            //按钮事件
            ButtonClick();
        }
        EditorGUILayout.EndHorizontal();
    }

    public void ButtonClick()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        
        StreamWriter writer = File.CreateText(filePath);
        writer.Write(scriptContent);
        writer.Close();
        
        AssetDatabase.Refresh();
        
        Debug.Log("生成脚本文件" + filePath);
        
        if(EditorUtility.DisplayDialog("自动化生成工具", "脚本生成成功", "确定"))
        {
            Close();
        }
    }
    
    /// <summary>
    /// 获取插入代码的下标
    /// </summary>
    /// <param name="content">原始脚本内容</param>
    /// <returns>插入位置的索引，如果未找到则返回-1</returns>
    public int GetInsertIndex(string content)
    {
        Regex regex = new Regex("UI组件事件");
        Match match = regex.Match(content);

        Regex regex1 = new Regex("public");
        MatchCollection matchCollection = regex1.Matches(content);
        for (int i = 0; i < matchCollection.Count; i++)
        {
            if (matchCollection[i].Index > match.Index)
            {
                return matchCollection[i].Index;
            }
        }
        return -1;
    }
}
