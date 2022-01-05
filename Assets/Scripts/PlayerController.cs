using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float invinciTime;

    private bool isWalk = false;
    private bool isJump = false;
    private bool isGround = false;

    [SerializeField] private GameManager gameManager;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private CapsuleCollider2D capsuleCollider;




    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
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
        MoveAnim();
        CheckGround();
        TimeCount();
    }

    private void TimeCount() {
    }


    private void Move() {
        // Move
        float h = Input.GetAxisRaw("Horizontal");

        if (h == 0) {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
        else {
            rigid.AddForce(Vector2.right * h * rigid.gravityScale * speed, ForceMode2D.Impulse);

            if (rigid.velocity.x > maxSpeed) {  // right
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            }
            else if (rigid.velocity.x < -maxSpeed) {    // left
                rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);
            }
        }
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
        if (rigid.velocity.y <= 0) {
            Debug.DrawRay(rigid.position, 
                Vector2.down,
                new Color(0, 1, 0));
            
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, 
                                    Vector2.down, 
                                    capsuleCollider.bounds.extents.y + 0.2f,
                                    LayerMask.GetMask("Platform"));
            
            if (rayHit.collider != null) {
                isJump = false;
                isGround = true;
                anim.SetBool("isJump", isJump);
            } 
            else {
                isJump = true;
                isGround = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy") {
            if(rigid.velocity.y < 0 && transform.position.y > other.transform.position.y)
                OnAttack(other.transform);
            else
                OnDamaged(other.transform.position);
        }
        else if (other.gameObject.tag == "Trap")
            OnDamaged(other.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Item") {
            ItemController itemController = other.GetComponent<ItemController>();
            
            // Deactive Item
            itemController.DeactiveItem();
        }
        else if (other.gameObject.tag == "Finish") {
            gameManager.NextStage();
        }
    }


    private void OnAttack(Transform enemy) {
        AttackReaction();

        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController.OnDamaged();
    }

    private void AttackReaction() {
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    }


    private void OnDamaged(Vector2 targetPos) {
        gameManager.HealthDown();

        gameObject.layer = 9;
        
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        int direction = transform.position.x - targetPos.x > 0 ? 1 : -1;

        rigid.AddForce(new Vector2(direction, 1) * 7, ForceMode2D.Impulse);

        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", invinciTime);
    }

    private void OffDamaged() {
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }

    public void OnDead() {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        capsuleCollider.enabled = false;

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero() {
        capsuleCollider.attachedRigidbody.velocity = Vector2.zero;
    }
}
