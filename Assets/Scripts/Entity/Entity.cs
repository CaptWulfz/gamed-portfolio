using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public enum EntityState
{
    NORMAL,
    HURT,
    INVINCIBLE
}

public class Entity : MonoBehaviour
{
    [Header("Entity Settings")]
    [SerializeField] protected Rigidbody2D rigidBody;
    [SerializeField] protected GroundCheck groundCheck;

    public Rigidbody2D RigidBody
    {
        get { return this.rigidBody; }
    }

    public GroundCheck GroundCheck
    {
        get { return this.groundCheck; }
    }

    protected string entityId;
    protected string audioSourceName;

    protected bool canApplyGravity;

    protected StateManager<EntityState> entityStateManager;
    public StateManager<EntityState> EntityStateManager
    {
        get { return this.entityStateManager; }
    }

    public EntityState CurrentState
    {
        get { return this.entityStateManager.CurrentState; }
    }

    private Controls controls;
    public Controls EntityControls
    {
        get { return this.controls; }
        set { this.controls = value; }
    }

    protected string sourceName = "";
    private float speed = 5f;
    public float Speed
    {
        get { return this.speed; }
        set { this.speed = value; }
    }

    protected Dictionary<string, object> animatorParameters;

    protected virtual void Initialize()
    {
        this.rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
        this.audioSourceName = string.Format("{0}@{1}", this.entityId, System.Guid.NewGuid());

        this.EntityControls = InputManager.Instance.GetControls();
        this.canApplyGravity = false;
        InitializeEntityStateManager();
    }

    protected void InitializeEntityStateManager()
    {
        this.entityStateManager = new StateManager<EntityState>();
        this.entityStateManager.Initialize(new List<EntityState> { EntityState.NORMAL, EntityState.HURT, EntityState.INVINCIBLE }, EntityState.NORMAL, true);
        this.entityStateManager.MakeTransition(EntityState.NORMAL, EntityState.HURT);
        this.entityStateManager.MakeTransition(EntityState.NORMAL, EntityState.INVINCIBLE);
        this.entityStateManager.MakeTransition(EntityState.HURT, EntityState.INVINCIBLE);
        this.entityStateManager.MakeTransition(EntityState.INVINCIBLE, EntityState.NORMAL);
    }

    protected void MovePosition(Vector2 move)
    {
        this.rigidBody.velocity = move;
    }

    protected void AddVelocity(Vector2 add)
    {
        this.rigidBody.velocity += add;
    }
    protected void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
    {
        this.rigidBody.AddForce(force, mode);
    }

    protected void FollowTarget(Transform target, float speed)
    {
        this.rigidBody.MovePosition(Vector2.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime));
    }

    #region Collision Events
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {

    }
    #endregion
}

public enum Direction
{
    RIGHT,
    LEFT
}
