using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class World
{
    public void AddLogicCtrl(ILogicBehaviour logicBehaviour)
    {
        mLogicBeahaviourDic.Add(logicBehaviour.GetType().Name, logicBehaviour);
        logicBehaviour.OnCreate();
    }
    
    public void AddDataMgr(IDataBehaviour dataBehaviour)
    {
        mDataBeahaviourDic.Add(dataBehaviour.GetType().Name, dataBehaviour);
        dataBehaviour.OnCreate();
    }
    
    public void AddMsgMgr(IMsgBehaviour msgBehaviour)
    {
        mMsgBeahaviourDic.Add(msgBehaviour.GetType().Name, msgBehaviour);
        msgBehaviour.OnCreate();
    }
}
