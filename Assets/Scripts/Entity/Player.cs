using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity, IMovable, IJumpable, IDashable, IGlideable, IGravityAffected
{
    #region IMovable Variables
    public bool canMove { get; set; }
    #endregion

    #region IJumpable Variables
    public float jumpForce { get; set; }
    public bool isJumping { get; set; }
    public bool canDoubleJump { get; set; }
    public bool hasDoubleJumped { get; set; }
    #endregion

    #region IDashable Variables
    public float dashForce { get; set; }
    public bool canDash { get; set; }
    public bool isDashing { get; set; }
    #endregion

    #region IGlideable Variables
    public bool isGliding { get; set; }
    #endregion

    private bool jumpPressed = false;
    public bool JumpPressed
    {
        get { return this.jumpPressed; }
    }

    private Direction direction;
    public bool IsDownPressed { get; set; }
    public bool PassThrough { get; set; }
    public Action StructureAction { get; set; }
    public Action NPCAction { get; set; }

    private const float FALL_MULTIPLIER = 750f;
    private const float JUMP_MULTIPLIER = 550f;
    private const float LOW_JUMP_MULTIPLIER = 1500f;

    private SpriteRenderer sr;
    private Animator a;

    void Start()
    {
        Initialize();
        this.Speed = 700f;
        this.jumpForce = 2100f;
        this.dashForce = 2500f;

        this.canMove = true;
        this.canDoubleJump = true;
        this.hasDoubleJumped = false;

        this.canApplyGravity = true;

        this.canDash = true;
        this.isDashing = false;

        this.direction = Direction.RIGHT;
        this.EntityControls.Player.Enable();

        //GameManager.Instance.Initialize();
        GameManager.Instance.RegisterPlayerToGameManager(this.GetComponent<Player>());
        GameManager.Instance.MakeGameCameraFollowTarget(this.transform);

        sr = this.transform.GetComponentInChildren<SpriteRenderer>();
        a = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.groundCheck.IsTouchingGround)
        {
            a.SetBool("grounded", true);
            this.hasDoubleJumped = false;
            this.rigidBody.gravityScale = 1000f;
        } else {
            a.SetBool("grounded", false);
            this.rigidBody.gravityScale = 10f;
        }

        //if (this.rigidBody.velocity.y >= 0)
        //    this.rigidBody.gravityScale = 10;
        //else if (this.rigidBody.velocity.y < 0)
        //    this.rigidBody.gravityScale = 10;
    }

    void FixedUpdate()
    {
        ApplyGravity();
        a.SetFloat("x", this.rigidBody.velocity.x);
        a.SetFloat("y", this.rigidBody.velocity.y);
        if (this.rigidBody.velocity.y < 1000)
            JumpRelease();
    }

    #region Player Specific Functions
    public void TriggerStructureAction()
    {
        if (this.StructureAction == null)
            return;

        if (!this.groundCheck.IsTouchingGround)
            return;

        this.RigidBody.velocity = Vector2.zero;
        this.StructureAction?.Invoke();
        this.StructureAction = null;
    }

    public void TryNPCAction()
    {
        if (this.NPCAction == null)
            return;

        if (!this.groundCheck.IsTouchingGround)
            return;

        this.NPCAction?.Invoke();
        //this.NPCAction = null;
    }

    public void ToggleDownPressed(bool pressed)
    {
        this.IsDownPressed = pressed;
    }
    #endregion

    #region IMovable Functions
    public void Move(float xPos)
    {
        if (xPos > 0) {
            this.direction = Direction.RIGHT;
            sr.flipX = false;
        } else if (xPos < 0) {
            this.direction = Direction.LEFT;
            sr.flipX = true;
        }

        if (!this.isDashing)
        {
            float speedDivisor = this.isGliding && !this.groundCheck.IsTouchingGround ? 2 : 1;
            Vector2 move = new Vector2((xPos * this.Speed) / speedDivisor, this.rigidBody.velocity.y);
            this.MovePosition(move);
        }
    }
    #endregion

    #region IGravityAffected Functions
    public void ApplyGravity()
    {
        if (this.groundCheck.IsTouchingGround)
            return;
        if (this.isDashing)
            return;

        Rigidbody2D rb = this.rigidBody;

        if (rb.velocity.y < 0)
        {
            if (this.isGliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, -300f);
                rb.gravityScale = 1f;
                return;
            } else
            {
                Vector2 gravity = Vector2.up * Physics2D.gravity.y * FALL_MULTIPLIER * Time.deltaTime;
                this.AddVelocity(gravity);
            }
        }
        else if (rb.velocity.y > 0 && this.jumpPressed)
        {
            Vector2 gravity = Vector2.up * Physics2D.gravity.y * JUMP_MULTIPLIER * Time.deltaTime;
            this.AddVelocity(gravity);
        }
        else if (rb.velocity.y > 0 && !this.jumpPressed)
        {
            Vector2 gravity = Vector2.up * Physics2D.gravity.y * LOW_JUMP_MULTIPLIER * Time.deltaTime;
            this.AddVelocity(gravity);
        }
    }
    #endregion

    #region IJumpable Functions
    public void Jump()
    {
        // Pass Through Functionality
        if (this.IsDownPressed && this.groundCheck.IsTouchingGround)
        {
            this.PassThrough = true;
            return;
        }

        // Jumping Functionality
        if (!this.groundCheck.IsTouchingGround && this.hasDoubleJumped)
        {
            return;
        } else if (!this.groundCheck.IsTouchingGround && !this.hasDoubleJumped)
        {
            hasDoubleJumped = true;
        }

        if (this.isGliding)
        {
            GlideRelease();
        }

        this.jumpPressed = true;
        a.SetBool("jumping", true);

        //Vector2 jumpMove = new Vector2(this.rigidBody.velocity.x, 1f * this.jumpForce);
        //this.MovePosition(jumpMove);
        //this.rigidBody.gravityScale = 10;
        this.rigidBody.velocity = new Vector2(this.rigidBody.velocity.x, 0);
        this.AddForce(Vector2.up * this.jumpForce, ForceMode2D.Impulse);
    }
    #endregion

    public void JumpRelease()
    {
        this.jumpPressed = false;
        this.PassThrough = false;
        a.SetBool("jumping", false);
    }

    #region IDashable Functions
    public void Dash()
    {
        if (!this.canDash)
            return;

        //Vector2 dashMove = new Vector2(this.EntityControls.Player.Move.ReadValue<float>() * this.dashForce, 0);
        //this.MovePosition(dashMove);

        if (this.isGliding)
        {
            GlideRelease();
        }

        Vector2 dashDir = this.direction == Direction.RIGHT ? Vector2.right : Vector2.left;
        this.rigidBody.velocity = new Vector2(0, 0);
        this.entityStateManager.SwitchState(EntityState.INVINCIBLE);
        //Debug.Log("Current State After Dashing: " + this.entityStateManager.CurrentState);
        this.AddForce(dashDir * this.dashForce, ForceMode2D.Impulse);
        this.a.SetTrigger("dash");
        StartCoroutine(DashWait());
    }

    public IEnumerator DashWait()
    {
        this.rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY;
        this.rigidBody.freezeRotation = true;
        this.isDashing = true;
        a.SetBool("dashing", true);

        yield return new WaitForSeconds(0.15f);

        this.rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        this.rigidBody.velocity = Vector2.zero;
        this.isDashing = false;
        a.SetBool("dashing", false);
        this.canDash = false;
        StartCoroutine(DelayDashReturnToNormalState());

        yield return new WaitForSeconds(1f);

        yield return new WaitUntil(() => { return this.groundCheck.IsTouchingGround; });
        canDash = true;
    }
    #endregion

    private IEnumerator DelayDashReturnToNormalState()
    {
        yield return new WaitForSeconds(0.2f);
        this.EntityStateManager.SwitchState(EntityState.NORMAL);
        //Debug.Log("Current State After Dash End: " + this.entityStateManager.CurrentState);
    }

    #region IGlideable Functions
    public void Glide()
    {
        if (this.groundCheck.IsTouchingGround)
            return;

        this.isGliding = true;
        a.SetTrigger("glide");
        a.SetBool("gliding", true);
    }

    public void GlideRelease()
    {
        this.isGliding = false;
        this.rigidBody.gravityScale = 10f;
        a.SetBool("gliding", false);
    }
    #endregion
}
