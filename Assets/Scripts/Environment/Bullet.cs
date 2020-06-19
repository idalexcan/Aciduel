using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour // ---- C L A S E    B U L L E T -----------------------------------------
{   
    int timelife=0;
    public bool droped, onCorrosion;
    public Queue<GameObject> myQueue = new Queue<GameObject>();
    void Update()
    {
        if (droped)
        {
            timelife++;
            if (timelife==70 || onCorrosion)
            {
                Control.Introducing(gameObject, myQueue);
                
                timelife=0;
                droped=false;
                onCorrosion=false;
            }
        }
    }
}
