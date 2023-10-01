using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed;

    public Animator anim;

    public static PlayerController instance;

    public string areaTransitionName;

    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    public bool canMove = true;

    public Vector2 moveInput;

    [Header("World Interaction")]
    public bool isIndoors;
    public bool stairsFacingRight;
    public bool onSideFacingStairs;
    public Vector2 stairSpeed;


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
        if(canMove)
        {
            //Are we on stairs or not?
            if(onSideFacingStairs && Input.GetKey(KeyCode.A) || onSideFacingStairs && Input.GetKey(KeyCode.D))
            {
                if(stairsFacingRight)
                {
                    if(Input.GetAxisRaw("Horizontal") == 1)
                    {
                        rb.velocity = (moveInput += stairSpeed) * moveSpeed;
                    }
                    if(Input.GetAxisRaw("Horizontal") == -1)
                    {
                        rb.velocity = (moveInput -= stairSpeed) * moveSpeed;
                    }
                    
                }
                else
                {
                    if(Input.GetAxisRaw("Horizontal") == -1)
                    {
                        rb.velocity = (moveInput += stairSpeed) * moveSpeed;
                    }
                    if(Input.GetAxisRaw("Horizontal") == +1)
                    {
                        rb.velocity = (moveInput -= stairSpeed) * moveSpeed;
                    }
                }
            }
            else
            {
                //THIS IS THE MAIN MOVE INPUTS
                rb.velocity = moveInput * moveSpeed;
            }

            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            anim.SetFloat("x", moveInput.x);
            anim.SetFloat("y", moveInput.y);
            anim.SetFloat("Speed", moveInput.sqrMagnitude);

            if(Input.GetAxisRaw("Horizontal")== 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                anim.SetFloat("LastHorizontal", Input.GetAxisRaw("Horizontal"));
                anim.SetFloat("LastVertical", Input.GetAxisRaw("Vertical"));
            }
        }
        else
        {
            //need to cancel out velocity because otherwise the rb can keep moving in some situations
            rb.velocity = Vector2.zero;
            anim.SetFloat("Speed", 0);
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
