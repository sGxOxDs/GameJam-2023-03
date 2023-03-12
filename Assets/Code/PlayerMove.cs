using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float movespeed = 5f;
    GameObject current;//���󪺦s���ܼ�
    // Start is called before the first frame update
    [SerializeField] float jupmespeed = 2f;
    // public float jumpcd=1f;//�Ψӳ]�w���D�ɬI���D�����O
    // public float jumptime=0f;
    // public float jumpForce = 6f; // �Ψӳ]�w���D�ɬI���D�����O
    public Transform footPoint; //�ΨӰO��Player����}�U���I�A�]��public�ñqInspector�����]�w
    public bool touchGround; //�h�O�ΨӰO���O�_Ĳ�a�����L�ܼ�
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
    //             // other.contacts[0].point//���D�I������I���y��
    //             // other.contacts[1].normal���D�v�u���m�A²������N�O��
    //             Debug.Log("����F");
    //             current = other.gameObject;
    //         }


    //     }
    //     else if(other.gameObject.tag == "nails")
    //     {
    //         Debug.Log("����");
    //         current = other.gameObject;
    //     }
    //     else if(other.gameObject.tag == "top")
    //     {
    //         Debug.Log("����Ѫ�O�F");
    //         current.GetComponent<BoxCollider2D>().enabled = false;//²�����current�ܼƴN�O���D�ثe�I�쪺�O�H��
    //         //����������BoxCollider2D �򥻤W�N�O�̭��Ҧ��\�઺���Denabled = false�����Ŀ� true �Ŀ�
    //     }

    // }
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.gameObject.tag == "rip")
    //     {
    //         Debug.Log("���F");
    //     }

    // }

}
