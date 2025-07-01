using System.Collections;
using System.Collections.Generic;
using GC.Hall;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WorldManager.CreateWorld<HallWorld>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
