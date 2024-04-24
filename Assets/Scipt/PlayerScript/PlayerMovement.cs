using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviourPun
{
    PlayerStatus pStat;
    PhotonView PV;
    Rigidbody2D rb2d;
    Animator animator;
    // Var-Move
    private float hMove;
    [SerializeField] private float movementSpeed = 10f;
    private bool facingRight = true;

    // Var-Jump
    protected JumpButton jumpButton;
    private bool isJumpingPressed = false;
    public float jumpForce = 20f;
    
    private bool isGrounded;
    public Transform feetPos,feetPos2;
    public float checkRadius = 0.3f;
    public LayerMask groundIdentity;
    
    private int jumpChance = 2;
    private int jumpLeft;
    private float delayDoubleJump = 0.4f;
    private float delayFallJump = 0.22f;
    private float jumpTimer;

    // Var-Attack
    protected AttackButton attackButton;
    private bool isAttackingPressed;
    private float currentTimeBetweenAttack;
    private float timeBetweenAttack = 0.4f;
    // Var-Punch-Attack
    public float punchRange;
    public Transform punchPos;
    public LayerMask enemyLayer;
    
    [SerializeField] private float knockBackForce = 2f;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();

        
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
        if (PV.IsMine)
        {
            pStat = GetComponent<PlayerStatus>();
            rb2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            jumpButton = FindObjectOfType<JumpButton>();
            attackButton = FindObjectOfType<AttackButton>();
            gameObject.layer = 8;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        GetMoveInput();
        GetJumpInput();
        GetAttackInput();
        Animate();
    }
    
    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        groundCheck();
        Punch();
        Move();
        Jump();
    }

    // Input-Move
    private void GetMoveInput()
    {
        hMove = CrossPlatformInputManager.GetAxis("Horizontal");
    }
    // Move()
    private void Move()
    {
        rb2d.velocity = new Vector2(hMove * movementSpeed, rb2d.velocity.y);
        Flip();
    }
    private void Flip()
    {
        if(hMove > 0 && !facingRight)
        {
            Vector3 oldScale = transform.localScale;
            oldScale.x = -oldScale.x;
            transform.localScale = oldScale;
            facingRight = true;
        }
        else if (hMove < 0 && facingRight)
        {
            Vector3 oldScale = transform.localScale;
            oldScale.x = -oldScale.x;
            transform.localScale= oldScale;
            facingRight = false;
        }
    }
    
    // Input-Jump
    private void GetJumpInput()
    {
        if (jumpButton.isPressed)
        {
            isJumpingPressed = true;
        }
        else if (!jumpButton.isPressed)
        {
            isJumpingPressed = false;
        }
        if (!isGrounded)
        {
            jumpTimer -= Time.deltaTime;
        }
        
    }

    // Jump()
    private void Jump()
    {
        
        if (!isGrounded && jumpLeft == 2)
        {
            jumpLeft -= 1;
            jumpTimer = delayFallJump;
        }
        if (isGrounded)
        {
            jumpLeft = jumpChance;
        }
        if (isJumpingPressed)
        {
            if(isGrounded && jumpLeft ==2 )
            {
                rb2d.velocity = Vector2.up * jumpForce;
                jumpLeft -= 1;
                
                jumpTimer = delayDoubleJump;
            }
            if (jumpLeft == 1 && jumpTimer < 0 && !isAttackingPressed)
            {
                rb2d.velocity = Vector2.up * jumpForce;
                jumpLeft -=1 ;
            }
        }
    }
    // Get Attack Input
    private void GetAttackInput()
    {
        currentTimeBetweenAttack -= Time.deltaTime;
        
        if (!attackButton.isPressed)
        {
            isAttackingPressed = false;
        }
        else if  (attackButton.isPressed )
        {
            if(currentTimeBetweenAttack < 0)
            {
                isAttackingPressed = true; currentTimeBetweenAttack = timeBetweenAttack;
            }
            else
            {
                isAttackingPressed = false;
            }
            
        }

    }
    // Punch
    void Punch()
    {
        if (isAttackingPressed)
        {
            Collider2D[] enemyToDamage = Physics2D.OverlapCircleAll(punchPos.position, punchRange, enemyLayer);
            foreach (Collider2D enemy in enemyToDamage)
            {
                //enemy.gameObject.GetComponent<PlayerMovement>().TakeDamage(pStat.GetAttackDamage(),transform.position-enemy.gameObject.transform.position);
                enemy.gameObject.GetComponent<PlayerMovement>().TakeDamage(pStat.GetAttackDamage());
            }
        }
    }

    // Ground check
    private void groundCheck()
    {
        if (Physics2D.OverlapCircle(feetPos.position, checkRadius, groundIdentity) && rb2d.velocity.y == 0 || Physics2D.OverlapCircle(feetPos2.position, checkRadius, groundIdentity) && rb2d.velocity.y == 0 ||
            Physics2D.OverlapCircle(feetPos.position, checkRadius, enemyLayer) && rb2d.velocity.y == 0 || Physics2D.OverlapCircle(feetPos2.position, checkRadius, enemyLayer) && rb2d.velocity.y == 0 )
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    // Animate
    public void Animate()
    {
        animator.SetFloat("speed", Mathf.Abs(rb2d.velocity.x));
        animator.SetFloat("yVelocity", rb2d.velocity.y);
        animator.SetBool("isGrounded", isGrounded);
        if (isAttackingPressed) {
            animator.SetTrigger("animAttackTrigger");
        }
    }

    //public void TakeDamage(int damage,Vector2 dir)
    public void TakeDamage(int damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
        
    }

    [PunRPC]
    //void RPC_TakeDamage(int damage, Vector2 dir)
    void RPC_TakeDamage(int damage)
    {
        if (!PV.IsMine)
        {
            return;
        }
        pStat.TakeDamage(damage);
        //rb2d.AddForce(dir.normalized * knockBackForce);
    }

    
}
