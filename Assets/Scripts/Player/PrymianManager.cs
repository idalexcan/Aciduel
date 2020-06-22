using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PrymianManager : MonoBehaviourPun //,IPunObservable
{
    Prymian prymian;
    public GameObject[] toDelete;

    public int cantbullets, shots=0;
    public GameObject bulletsContainer, shotpoint;
    public Queue<GameObject> bullets = new Queue<GameObject>();

    public int life = 69; // yiyi yoyosilve 

    void Awake()
    {
        // this.photonView.RPC("RpcSelectTeam", RpcTarget.AllBuffered);
        prymian=GetComponent<Prymian>();

        if (!this.photonView.IsMine)//No soy yo, soy mimismo
        {
            Destroy(prymian);
            foreach (var item in toDelete)
            {
                Destroy(item.gameObject);
            }
        }
        else
        {
            // FindObjectOfType<Text>().text = transform.tag;
            this.photonView.RPC("RpcInstancier", RpcTarget.AllBuffered);
        }
    }

    [PunRPC] void RpcInstancier()
    {
        cantbullets=bulletsContainer.transform.childCount;
        for (int i = 0; i < cantbullets; i++)
        {
            bullets.Enqueue(bulletsContainer.transform.GetChild(i).gameObject);
            bulletsContainer.transform.GetChild(i).gameObject.AddComponent<AcidBall>().mng=this;
            bulletsContainer.transform.tag=transform.tag;
            bulletsContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
        DontDestroyOnLoad(bulletsContainer);
    }

    [PunRPC] public void Shot()
    {
        this.photonView.RPC("RpcShot", RpcTarget.All);
    }
    [PunRPC] void RpcShot()
    {
        GameObject go=bullets.Dequeue();
        go.SetActive(true); 
        go.transform.position=shotpoint.transform.position;
        go.transform.eulerAngles=shotpoint.transform.eulerAngles;
        go.GetComponent<Rigidbody>().AddForce(shotpoint.transform.forward*1200);
        go.GetComponent<Rigidbody>().useGravity= true;
        go.GetComponent<AcidBall>().droped= true;
        go.GetComponent<AcidBall>().myQueue=bullets;
        AudioSource.PlayClipAtPoint(prymian.shotClip, transform.position);
        shots++;
    }

    public static void Introducing(GameObject go, Queue<GameObject> list)
    {
        list.Enqueue(go.gameObject);
        go.GetComponent<Rigidbody>().velocity=Vector3.zero;
        go.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(life);
        }
        else if (stream.IsReading)
        {
            life = (int)stream.ReceiveNext();
        }
    }
}
