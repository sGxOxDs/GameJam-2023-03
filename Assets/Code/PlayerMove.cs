using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float movespeed = 5f;
    GameObject current;//物件的存放變數
    // Start is called before the first frame update
    [SerializeField] float jupmespeed = 2f;
    // public float jumpcd=1f;//用來設定跳躍時施予主角的力
    // public float jumptime=0f;
    // public float jumpForce = 6f; // 用來設定跳躍時施予主角的力
    public Transform footPoint; //用來記錄Player物件腳下的點，設為public並從Inspector中做設定
    public bool touchGround; //則是用來記錄是否觸地的布林變數
    float m_Speed = 10f;
    public BoxCollider2D myFeet;
    public Rigidbody2D myLeft;

    public Vector2 inputVector2 = Vector2.zero;
    void Start()
    {
        myFeet = GetComponent<BoxCollider2D>();
        myLeft = GetComponent<Rigidbody2D>();
        // myRigibody = GetComponent<Rigibody2D>();
    }

    public void OnMove(InputValue inputValue)
    {
        inputVector2 = inputValue.Get<Vector2>();
    }

    void Update()
    {

        Vector2 tanpv = myLeft.velocity;
        tanpv.x = (Vector2.right * inputVector2).x * movespeed;
        myLeft.velocity = tanpv;
        jump();
        checkedGrounded();
        Debug.Log(myLeft.velocity);
        // if(Input.GetKey(KeyCode.UpArrow))
        // {

        //     jump();
        // }
        // jumptime+=Time.deltaTime;
    }
    void checkedGrounded()
    {
        touchGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Floor"));
        Debug.Log("Floor");
    }
    void jump()
    {
        if (inputVector2.y == 1)
        {
            if (touchGround)
            {
                //  transform.GetComponent<Rigidbody2D>().AddForce(transform.up * 10f, ForceMode2D.Impulse);
                myLeft.velocity = Vector2.up * jupmespeed;
            }
            //  myLeft.velocity + 
        }
        // if(jumptime>jumpcd)
        // {
        //     jumptime=0;
        //     transform.GetComponent<Rigidbody2D>().AddForce(transform.up * 10f, ForceMode2D.Impulse);
        // }



    }
    // void OnCollisionEnter2D(Collision2D other)
    // {
    //     if(other.gameObject.tag == "normal")
    //     {
    //         if(other.contacts[0].normal == new Vector2(0f,1f))
    //         {
    //             // Debug.Log(other.contacts[0].normal);
    //             // Debug.Log(other.contacts[1].normal);
    //             // other.contacts[0].point//知道碰到哪個點的座標
    //             // other.contacts[1].normal知道髮線鏈位置，簡單來講就是邊
    //             Debug.Log("撞到了");
    //             current = other.gameObject;
    //         }


    //     }
    //     else if(other.gameObject.tag == "nails")
    //     {
    //         Debug.Log("受傷");
    //         current = other.gameObject;
    //     }
    //     else if(other.gameObject.tag == "top")
    //     {
    //         Debug.Log("撞到天花板了");
    //         current.GetComponent<BoxCollider2D>().enabled = false;//簡單來講current變數就是知道目前碰到的是沈麼
    //         //取的父物件BoxCollider2D 基本上就是裡面所有功能的標題enabled = false取消勾選 true 勾選
    //     }

    // }
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.gameObject.tag == "rip")
    //     {
    //         Debug.Log("死了");
    //     }

    // }

}
