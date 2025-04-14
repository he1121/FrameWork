using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GeneratorFindComponentTool:Editor
{

    public static Dictionary<int, string> objFindPathDic; // k 物体的instance id， v 物体的查找路径
    public static List<EditorObjectData> objDataList; //查找对象的数据
    [MenuItem("GameObject/生成组件查找脚本")]
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
    }

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
                Transform parent = trans.parent;
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
    
    public class EditorObjectData
    {
        public int instanceID;
        public string fieldName;
        public string fieldType;
    }
}
