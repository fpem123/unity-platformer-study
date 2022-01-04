using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int nextMove;
    private float currentTime;

    [SerializeField] private float idleTimeMin;
    [SerializeField] private float idleTimeMax;
    [SerializeField] private float walkTimeMin;
    [SerializeField] private float walkTimeMax;

    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    Animator anim;


    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Think();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move();
        CheckPlatform();
        TimeCount();
    }

    private void TimeCount() {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
            Think();
    }

    private void Move() {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
    }

    void Think() {
        nextMove = Random.Range(-1, 2);
        anim.SetInteger("walkSpeed", nextMove);

        switch (nextMove)
        {
            case 0:
                currentTime = Random.Range(idleTimeMin, idleTimeMax);
                break;
            default:
                currentTime = Random.Range(walkTimeMin, walkTimeMax);
                spriteRenderer.flipX = nextMove == 1;
                break;
        }
    }

    void CheckPlatform() {
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);

        Debug.DrawRay(frontVec, 
                Vector2.down,
                new Color(0, 1, 0));
            
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, 
                                Vector2.down, 
                                capsuleCollider.bounds.extents.y + 0.2f,
                                LayerMask.GetMask("Platform"));
        
        if (rayHit.collider == null){
            currentTime = 0;
        }
    }
}
