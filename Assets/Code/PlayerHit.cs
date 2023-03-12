using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] GameObject ButtonRE;
    PlayerMovement movement;
    private Animator anim;

    [SerializeField] UnityEvent dieEvent;
    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == gameObject.layer && movement.enabled)
        {
            ButtonRE.SetActive(true);
            movement.moveInput = Vector2.zero;
            movement.enabled = false;
            anim.SetTrigger("die");
            dieEvent?.Invoke();
        }
    }
}
