using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager
{
    private static List<World> mWorldList = new List<World>();
    /// <summary>
    /// 默认游戏世界
    /// </summary>
    public static World DefaultWorld{get; private set;}
    
    /// <summary>
    /// 创建游戏世界
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void CreateWorld<T>() where T : World, new()
    {
        T world = new T();
        DefaultWorld = world;
        
        //初始化当前世界的程序集
        TypeManager.InitializeWorldAssemblies(world, GetBehaviourExecution(world));
        world.OnCreate();
        mWorldList.Add(world);
    }

    /// <summary>
    /// 销毁指定的游戏世界
    /// </summary>
    /// <param name="world"></param>
    /// <typeparam name="T"></typeparam>
    public static void DestroyWorld<T>(World world) where T : World
    {
        foreach (var _world in mWorldList)
        {
            if (_world == world)
            {
                _world.DestroyWorld(typeof(T).Namespace);
                mWorldList.Remove(_world);
                break;
            }
        }
    }

    public static IBehaviourExecution GetBehaviourExecution(World world)
    {
        if (world.GetType().Name == "HallWorld")
        {
            return new HallWorldExecutionOrder();
        }

        return null;
    }
}
