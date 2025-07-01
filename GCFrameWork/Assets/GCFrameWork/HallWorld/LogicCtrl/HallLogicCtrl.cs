using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GC.Hall
{
    public class HallLogicCtrl : ILogicBehaviour
    {
        public void OnCreate()
        {
            Debug.Log("HallLogicCtrl OnCreate>>>>>>");
        }

        public void OnDestroy()
        {
            Debug.Log("HallLogicCtrl OnDestroy>>>>>>");
        }
    }
}
