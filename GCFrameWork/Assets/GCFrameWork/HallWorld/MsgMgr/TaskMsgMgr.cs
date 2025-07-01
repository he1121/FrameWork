using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GC.Hall
{
    public class TaskMsgMgr : IMsgBehaviour
    {
        public void OnCreate()
        {
            Debug.Log("TaskMsgMgr OnCreate>>>>>>");
        }

        public void OnDestroy()
        {
            Debug.Log("TaskMsgMgr OnDestroy>>>>>>");
        }
    }
}
