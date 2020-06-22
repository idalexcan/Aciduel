using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prymian : MonoBehaviour
{
    PrymianManager mng;
    // ------------------   |MOVEMENT|
    public bool play;
    int jumps;
    public bool onAir;
    public float speed;
    public float x,y,xr=0,yr=0,  upRotation;

    // ------------------   |AUDIO AND ANIMATION|
    Animator animator;
    public AudioClip shotClip, damaged;
    public AudioSource running;
    public float runningVol;

    /// -------------------------------------------------------------------------------<MONOBEHAVIOUR_FUNCTIONS>

    private void Awake()
    {
        mng=GetComponent<PrymianManager>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        play=true;
        animator=GetComponent<Animator>();
    }

    void Update()
    {
        if (play)
        {
            // ............comportamiento.
            MoveAndLook();
            Jump();
            if (Input.GetMouseButtonDown(0) && mng.shots<20)
            {
                mng.Shot();
            }

            // ........................................................animator.
            animator.SetBool("jump",Input.GetKeyDown(KeyCode.Space));
            animator.SetFloat("upMove", GetComponent<Rigidbody>().velocity.y);
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

    /// -------------------------------------------------------------------------------<PLAYER_HABILITIES>
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


}

/// <--------------------------------------------> || A C I D B A L L || <------------------------------------->
public class AcidBall : MonoBehaviour
{
    int timelife=0;
    public bool droped, onCorrosion;
    public Queue<GameObject> myQueue = new Queue<GameObject>();
    public PrymianManager mng;
    void Update()
    {
        if (droped)
        {
            timelife++;
            if (timelife==50 || onCorrosion)
            {
                PrymianManager.Introducing(gameObject, myQueue);
                
                timelife=0;
                droped=false;
                onCorrosion=false;

                if (mng!=null)
                {
                    mng.shots--;
                }
                
            }
        }
    }
}
