using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed;

    private Animator anim;

    public static PlayerController instance;

    public string areaTransitionName;

    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    public bool canMove = true;


    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
        

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        DontDestroyOnLoad(gameObject);
    }

    
    void Update()
    {
        //current movement and animation. Speed is not normalized diagonally. There is also no "Last Horizontal etc." so the idles snap forward.
        if(canMove)
        {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")) * moveSpeed;

            anim.SetFloat("x", rb.velocity.x);
            anim.SetFloat("y", rb.velocity.y);

            if(rb.velocity.x != 0 || rb.velocity.y != 0)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            //need to cancel out velocity because otherwise the rb can keep moving in some situations
            rb.velocity = Vector2.zero;
        }

        //Keeps player in bounds of tilemap - may change later depending on cinemachine

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);

    }

    //the tilemap bound variables are being set in the camera controller script
    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        bottomLeftLimit = botLeft + new Vector3(.5f, .5f, 0f);
        topRightLimit = topRight + new Vector3(-.5f, -.5f, 0); 
    }
}
