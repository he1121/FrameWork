using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class World
{
    /// <summary>
    /// 逻辑层所有类的字典
    /// </summary>
    private static Dictionary<string, ILogicBehaviour> mLogicBeahaviourDic = new Dictionary<string, ILogicBehaviour>();
    /// <summary>
    /// 数据层所有类的字典
    /// </summary>
    private static Dictionary<string, IDataBehaviour> mDataBeahaviourDic = new Dictionary<string, IDataBehaviour>();
    /// <summary>
    /// 消息层所有类的字典
    /// </summary>
    private static Dictionary<string, IMsgBehaviour> mMsgBeahaviourDic = new Dictionary<string, IMsgBehaviour>();
    
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void OnCreate(){}
    
    public virtual void OnUpdate(){}
    
    /// <summary>
    /// 世界销毁时触发
    /// </summary>
    public virtual void OnDestroy(){}

    /// <summary>
    /// 销毁世界
    /// </summary>
    public void DestroyWorld(string nameSpace, object args = null)
    {
        //统计需要销毁的逻辑层脚本
        List<string> needRemoveList = new List<string>();
        foreach (var logic in mLogicBeahaviourDic)
        {
            if (string.Equals(logic.Value.GetType().Namespace, nameSpace))
            {
                needRemoveList.Add(logic.Key);
            }
        }

        //释放逻辑层脚本
        foreach (var key in needRemoveList)
        {
            mLogicBeahaviourDic[key].OnDestroy();
            mLogicBeahaviourDic.Remove(key);
        }
        needRemoveList.Clear();
        
        //统计需要销毁的数据层脚本
        foreach (var data in mDataBeahaviourDic)
        {
            if (string.Equals(data.Value.GetType().Namespace, nameSpace))
            {
                needRemoveList.Add(data.Key);
            }
        }

        //释放数据层脚本
        foreach (var key in needRemoveList)
        {
            mDataBeahaviourDic[key].OnDestroy();
            mDataBeahaviourDic.Remove(key);
        }
        needRemoveList.Clear();
        
        //统计需要销毁的消息层脚本
        foreach (var msg in mMsgBeahaviourDic)
        {
            if (string.Equals(msg.Value.GetType().Namespace, nameSpace))
            {
                needRemoveList.Add(msg.Key);
            }
        }

        //释放消息层脚本
        foreach (var key in needRemoveList)
        {
            mMsgBeahaviourDic[key].OnDestroy();
            mMsgBeahaviourDic.Remove(key);
        }
        
        OnDestroy();
        OnDestroyPostProcess(args);
    }
    /// <summary>
    /// 世界销毁完成后触发
    /// </summary>
    /// <param name="arg"></param>
    public virtual void OnDestroyPostProcess(object arg){}

    /// <summary>
    /// 获取逻辑层控制器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetExistsLogicCtrl<T>() where T : ILogicBehaviour
    {
        ILogicBehaviour logicBehaviour = null;
        if (mLogicBeahaviourDic.TryGetValue(typeof(T).Name, out logicBehaviour))
        {
            return (T)logicBehaviour;
        }
        Debug.LogError("Get Class :" + typeof(T).Name +"failed!");
        return default(T);
    }
    
    /// <summary>
    /// 获取数据层管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetExistsDataCtrl<T>() where T : IDataBehaviour
    {
        IDataBehaviour dataBehaviour = null;
        if (mDataBeahaviourDic.TryGetValue(typeof(T).Name, out dataBehaviour))
        {
            return (T)dataBehaviour;
        }
        Debug.LogError("Get Class :" + typeof(T).Name +"failed!");
        return default(T);
    }
    
    /// <summary>
    /// 获取消息层管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetExistsMsgCtrl<T>() where T : IMsgBehaviour
    {
        IMsgBehaviour msgBehaviour = null;
        if (mMsgBeahaviourDic.TryGetValue(typeof(T).Name, out msgBehaviour))
        {
            return (T)msgBehaviour;
        }
        Debug.LogError("Get Class :" + typeof(T).Name +"failed!");
        return default(T);
    }
}
