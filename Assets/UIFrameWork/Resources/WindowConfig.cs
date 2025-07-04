﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "WindowConfig", menuName = "WindowConfig", order = 0)]
public class WindowConfig : ScriptableObject
{
    private string[] windowRootArr = new string[] { "Game", "Hall", "Window" };
    public List<WindowData> windowDataList = new List<WindowData>();

    public void GeneratorWindowConfig()
    {
        //检测预制体有没有新增，如果没有则不需要生成配置
        int count = 0;
        foreach (var item in windowRootArr)
        {
            //获取预制体文件夹读取路径
            string folder = Application.dataPath+"/UIFramework/Resources/" + item;
            //获取预制体文件夹下所有的预制体
            string[] prefabFiles = Directory.GetFiles(folder, "*.prefab", System.IO.SearchOption.AllDirectories);
            foreach (var path in prefabFiles)
            {
                if (path.EndsWith(".meta"))
                {
                    continue;
                }

                count += 1;
            }
        }

        if (count == windowDataList.Count)
        {
            return;
        }
        
        windowDataList.Clear();
        foreach (var item in windowRootArr)
        {
            //获取预制体文件夹读取路径
            string folder = Application.dataPath+"/UIFramework/Resources/" + item;
            //获取预制体文件夹下所有的预制体
            string[] prefabFiles = Directory.GetFiles(folder, "*.prefab", System.IO.SearchOption.AllDirectories);
            foreach (var path in prefabFiles)
            {
                if (path.EndsWith(".meta"))
                {
                    continue;
                }
                
                //获取预制体名字
                string fileName = Path.GetFileNameWithoutExtension(path);
                //获取预制体路径
                string filePath = item + "/" + fileName;
                WindowData data = new WindowData{name = fileName, path = filePath};
                windowDataList.Add(data);
            }
        }
    }

    public string GetWindowPath(string windowName)
    {
        foreach (var item in windowDataList)
        {
            if (string.Equals(item.name, windowName))
            {
                return item.path;
            }
        }

        Debug.LogError("不存在在配置文件中，请检查预制体存放位置或配置文件");
        return "";
    }
}

[System.Serializable]
public class WindowData
{
    public string name;
    public string path;
}