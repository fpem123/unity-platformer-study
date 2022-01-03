using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float jumpPower;

    private bool isWalk = false;
    private bool isJump = false;
    private bool isGround = false;

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

    // 단발성 입력
    private void Update() {
        SmoothStop();
        DirectionSprite();
        Jump();
    }

    // 지속적 입력
    private void FixedUpdate() {
        Move();
        CheckGround();
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


    private void Jump(){
        if (Input.GetButtonDown("Jump") && !isJump && isGround){
            isJump = true;
            isGround = false;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump", isJump);
        }
    }


    private void CheckGround(){
        if (rigid.velocity.y < 0) {
            Debug.DrawRay(rigid.position, 
                Vector2.down,
                new Color(0, 1, 0));
            
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, 
                                    Vector2.down, 
                                    1f,
                                    LayerMask.GetMask("Platform"));

            if (rayHit.collider != null && rayHit.distance < 0.5f) {
                isJump = false;
                isGround = true;
                anim.SetBool("isJump", isJump);
                Debug.Log(rayHit.collider.name);
            }
        }
    }

}
