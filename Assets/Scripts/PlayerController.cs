using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;

    private bool isWalk = false;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;



    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    private void Update() {
        SmoothStop();
        DirectionSprite();
    }


    private void FixedUpdate() {
        Move();
    }


    private void Move() {
        // Move
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) {  // right
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < -maxSpeed) {    // left
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);
        }

        MoveAnim();
    }

    private void MoveAnim() {
        if (Mathf.Abs(rigid.velocity.x) < 0.3f)
            isWalk = false;
        else
            isWalk = true;

        anim.SetBool("isWalk", isWalk);
    }


    private void SmoothStop(){
        if (Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, 
                                rigid.velocity.y);
        }    
    }

    private void DirectionSprite(){
        if (Input.GetKey(KeyCode.LeftArrow) && !spriteRenderer.flipX)
            spriteRenderer.flipX = true;
        else if (Input.GetKey(KeyCode.RightArrow) && spriteRenderer.flipX)
            spriteRenderer.flipX = false;
    }
}
