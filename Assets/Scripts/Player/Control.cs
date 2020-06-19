using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    // About shot ......................................................
    int cantbullets;
    public GameObject bulletsContainer, shotpoint;
    public static Queue<GameObject> bullets = new Queue<GameObject>();

    // About moving ......................................
    public bool play;
    int jumps;
    public bool onAir;
    public float speed;
    public float x,y,xr=0,yr=0,  upRotation;
    public Vector3 velocity;
    public GameObject underpoint;
    
    // About audio-visual ..............................
    Animator animator;
    public AudioClip shotClip, damaged;
    public AudioSource running;
    public float runningVol;
    


    // FUNCIONES DE MONOHEBAVIOUR ______________________________________________________________________________

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        play=true;
        animator=GetComponent<Animator>();

        cantbullets=bulletsContainer.transform.childCount;
        for (int i = 0; i < cantbullets; i++)
        {
            bullets.Enqueue(bulletsContainer.transform.GetChild(i).gameObject);
            bulletsContainer.transform.GetChild(i).gameObject.AddComponent<Bullet>();
            bulletsContainer.transform.GetChild(i).gameObject.SetActive(false);
        }

        DontDestroyOnLoad(bulletsContainer);
    }
    void Update()
    {
        if (play)
        {
            // variables.................................
            velocity=GetComponent<Rigidbody>().velocity;

            // comportamiento..
            MoveAndLook();
            Shot();
            Jump();

            // animator..............................................
            animator.SetBool("jump",Input.GetKeyDown(KeyCode.Space));
            animator.SetFloat("upMove", velocity.y);
            animator.SetBool("onAir",onAir);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.GetComponent<Tile>())
        {
            onAir=false;
            jumps=0;
        }
        
    }


    ///--------------------------------------------------------------<FUNCIONES_DE_MOVIMIENTO>
    void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumps++;
            if (jumps<3)
            {
                GetComponent<Rigidbody>().AddForce(transform.up*140);
                onAir=true;
            }
        }
    }

    void MoveAndLook()
    {
        x=Input.GetAxis("Horizontal");
        y=Input.GetAxis("Vertical");
        xr=Input.GetAxis("Mouse X");
        yr=Input.GetAxis("Mouse Y");

        if (upRotation<=0.3 && upRotation>=-0.3)
        {
            upRotation+=yr*0.01f;
        }else if (upRotation>0.3)
        {
            upRotation-=0.01f;
        }else if (upRotation<-0.3)
        {
            upRotation+=0.01f;
        }

        
        Rigidbody vel=GetComponent<Rigidbody>();
        float velUp=vel.velocity.y;
        Vector3 desiredVelocity = (x * transform.right + y * transform.forward * 2);
        vel.velocity=desiredVelocity*speed + transform.up*velUp;
        

        transform.Rotate(0,xr*2.6f,0);

        animator.SetFloat("upRotation", upRotation);
        animator.SetFloat("forwardMove", y);
        animator.SetFloat("rightMove", x);
        
        if (y!=0 || x!=0)
        {
            // running.Play();
            // running.PlayOneShot(running.clip);
            running.volume=runningVol;
        }else
        {
            running.volume=runningVol*0;
        }
        
    }

    ///--------------------------------------------------------------<FUNCIONES_DE_DISPARO>
    void Shot()
    {

        if (Input.GetMouseButtonDown(0))
        {
            GameObject go=bullets.Dequeue();
            go.SetActive(true); 
            go.transform.position=shotpoint.transform.position;
            go.GetComponent<Rigidbody>().AddForce(shotpoint.transform.forward*700);
            go.GetComponent<Rigidbody>().useGravity= true;
            go.GetComponent<Bullet>().droped= true;
            go.GetComponent<Bullet>().myQueue=bullets;
            AudioSource.PlayClipAtPoint(shotClip, transform.position);
        }
    }

    public static void Introducing(GameObject go, Queue<GameObject> list)
    {
        list.Enqueue(go.gameObject);
        go.GetComponent<Rigidbody>().velocity=Vector3.zero;
        go.SetActive(false);
    }

    

}

