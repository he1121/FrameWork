using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class TypeManager
{
    private static IBehaviourExecution mBehaviourExecution;
    public static void InitializeWorldAssemblies(World world, IBehaviourExecution behaviourExecution)
    {
        mBehaviourExecution = behaviourExecution;
        //获取unity和我们创建的脚本所在的程序集
        Assembly[] assembliesArr = AppDomain.CurrentDomain.GetAssemblies();
        Assembly worldAssembly = null;
        //获取当前脚本运行的程序集
        foreach (var assembly in assembliesArr)
        {
            if (assembly.GetName().Name == "Assembly-CSharp")
            {
                worldAssembly = assembly;
                break;
            }
        }

        if (worldAssembly == null)
        {
            Debug.LogError("worldAssembly is null!");
        }
        
        //先获取当前游戏世界的命名空间
        //再获取该命名空间下的所有脚本
        //判断脚本是否继承自自定义的xxxBehaviour，若继承，则是框架脚本，需维护其生命周期函数 
        string nameSpace = world.GetType().Namespace;
        Type logicBehaviourType = typeof(ILogicBehaviour);
        Type dataBehaviourType = typeof(IDataBehaviour);
        Type msgBehaviourType = typeof(IMsgBehaviour);
        //获取当前程序集下所有类型
        Type[] types = worldAssembly.GetTypes();
        List<TypeOrder> logicBehaviourList = new List<TypeOrder>();
        List<TypeOrder> dataBehaviourList = new List<TypeOrder>();
        List<TypeOrder> msgBehaviourList = new List<TypeOrder>();
        foreach (var type in types)
        {
            if (type.Namespace == nameSpace)
            {
                if (type.IsAbstract)
                    continue;
                if (logicBehaviourType.IsAssignableFrom(type)) //判断当前type是否继承自ILogicBehaviour
                {
                    //获取当前类的初始化顺序
                    int order = GetLogicBehaviourOrderIndex(type);
                    TypeOrder typeOrder = new TypeOrder(order, type);
                    logicBehaviourList.Add(typeOrder);
                }
                else if (dataBehaviourType.IsAssignableFrom(type))
                {
                    int order = GetDataBehaviourOrderIndex(type);
                    TypeOrder typeOrder = new TypeOrder(order, type);
                    dataBehaviourList.Add(typeOrder);
                }
                else if (msgBehaviourType.IsAssignableFrom(type))
                {
                    int order = GetMsgBehaviourOrderIndex(type);
                    TypeOrder typeOrder = new TypeOrder(order, type);
                    msgBehaviourList.Add(typeOrder);
                }
            }
        }
        //对逻辑层脚本、数据层脚本、消息层脚本进行升序排序
        logicBehaviourList.Sort((a, b)=>a.order.CompareTo(b.order));
        dataBehaviourList.Sort((a, b)=>a.order.CompareTo(b.order));
        msgBehaviourList.Sort((a, b)=>a.order.CompareTo(b.order));
        //初始化数据层脚本、消息层脚本、逻辑层脚本
        foreach (var typeOrder in dataBehaviourList)
        {
            IDataBehaviour dataBehaviour = Activator.CreateInstance(typeOrder.Type) as IDataBehaviour;
            world.AddDataMgr(dataBehaviour);
        }
        foreach (var typeOrder in msgBehaviourList)
        {
            IMsgBehaviour msgBehaviour = Activator.CreateInstance(typeOrder.Type) as IMsgBehaviour;
            world.AddMsgMgr(msgBehaviour);
        }
        foreach (var typeOrder in logicBehaviourList)
        {
            ILogicBehaviour logicBehaviour = Activator.CreateInstance(typeOrder.Type) as ILogicBehaviour;
            world.AddLogicCtrl(logicBehaviour);
        }
        logicBehaviourList.Clear();
        dataBehaviourList.Clear();
        msgBehaviourList.Clear();
        mBehaviourExecution = null;
    }

    private static int GetLogicBehaviourOrderIndex(Type type)
    {
        //获取逻辑层脚本的执行顺序
        if (mBehaviourExecution != null)
        {
            Type[] logicBehaviourExecutions = mBehaviourExecution.GetLogicBehaviourExecutions();
            for (int i = 0; i < logicBehaviourExecutions.Length; i++)
            {
                if (logicBehaviourExecutions[i] == type)
                {
                    //返回逻辑层脚本的执行顺序
                    return i;
                }
            }
        } 
        return 999;
    }
    
    private static int GetDataBehaviourOrderIndex(Type type)
    {
        //获取数据层脚本的执行顺序
        if (mBehaviourExecution != null)
        {
            Type[] dataBehaviourExecutions = mBehaviourExecution.GetDataBehaviourExecutions();
            for (int i = 0; i < dataBehaviourExecutions.Length; i++)
            {
                if (dataBehaviourExecutions[i] == type)
                {
                    //返回数据层脚本的执行顺序
                    return i;
                }
            }
        }
        return 999;
    }
    
    private static int GetMsgBehaviourOrderIndex(Type type)
    {
        //获取消息层脚本的执行顺序
        if (mBehaviourExecution != null)
        {
            Type[] msgBehaviourExecutions = mBehaviourExecution.GetMsgBehaviourExecutions();
            for (int i = 0; i < msgBehaviourExecutions.Length; i++)
            {
                if (msgBehaviourExecutions[i] == type)
                {
                    //返回数据层脚本的执行顺序
                    return i;
                }
            }
        }
        return 999;
    }
}
