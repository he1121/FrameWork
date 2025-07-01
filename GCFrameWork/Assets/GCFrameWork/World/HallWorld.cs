using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GC.Hall
{
    public class HallWorld :World
    {
        public override void OnCreate()
        {
            base.OnCreate();
            Debug.Log("HallWorld OnCreate>>>>>>");
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Debug.Log("HallWorld OnDestroy>>>>>>");
        }

        public override void OnDestroyPostProcess(object arg)
        {
            base.OnDestroyPostProcess(arg);
            Debug.Log("HallWorld OnDestroyPostProcess>>>>>>");
        }
    } 
}

