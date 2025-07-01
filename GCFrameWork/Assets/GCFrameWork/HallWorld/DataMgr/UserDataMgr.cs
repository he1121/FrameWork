using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GC.Hall
{
    public class UserDataMgr : IDataBehaviour
    {

        public void OnCreate()
        {
            Debug.Log("UserDataMgr OnCreate>>>>>>");
        }

        public void OnDestroy()
        {
            Debug.Log("UserDataMgr OnDestroy>>>>>>");
        }
    }
}
