using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ControlManager : MonoBehaviourPun
{
    public GameObject cam;
    Control control;
    void Start()
    {
        control=GetComponent<Control>();

        if (this.photonView.IsMine==false)
        {
            //Destroy(rott);
            control.enabled = false;
            cam.SetActive(false);
            //puntero.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
