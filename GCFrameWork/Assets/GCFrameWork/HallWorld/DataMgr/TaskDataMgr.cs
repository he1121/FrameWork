using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GC.Hall
{
    public class TaskDataMgr : IDataBehaviour
    {
        public void OnCreate()
        {
            Debug.Log("TaskDataMgr OnCreate>>>>>>");
        }

        public void OnDestroy()
        {
            Debug.Log("TaskDataMgr OnDestroy>>>>>>");
        }
    }
}
