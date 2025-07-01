using System;
using System.Collections;
using System.Collections.Generic;
using GC.Hall;
using UnityEngine;

public class HallWorldExecutionOrder:IBehaviourExecution
{
    private static Type[] LogicBehaviourExecutions = new Type[]
    {

    };
    
    private static Type[] DataBehaviourExecutions = new Type[]
    {
        typeof(UserDataMgr)
    };
    
    private static Type[] MsgBehaviourExecutions = new Type[]
    {

    };
    
    public Type[] GetLogicBehaviourExecutions()
    {
        return LogicBehaviourExecutions;
    }

    public Type[] GetDataBehaviourExecutions()
    {
        return DataBehaviourExecutions;
    }

    public Type[] GetMsgBehaviourExecutions()
    {
        return MsgBehaviourExecutions;
    }
}
