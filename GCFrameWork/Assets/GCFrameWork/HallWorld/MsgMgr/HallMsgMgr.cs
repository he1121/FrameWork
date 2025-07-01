using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GC.Hall
{
    public class HallMsgMgr : IDataBehaviour
    {
        public void OnCreate()
        {
            Debug.Log("HallMsgMgr OnCreate>>>>>>");
        }

        public void OnDestroy()
        {
            Debug.Log("HallMsgMgr OnDestroy>>>>>>");
        }
    }
}
