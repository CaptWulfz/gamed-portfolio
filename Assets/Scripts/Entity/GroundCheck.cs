using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Check Attributes")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] bool isTouchingGround = false;

    private bool invokeOnLand = false;
    public Action OnLand { get; set; }

    public bool IsTouchingGround
    {
        get { return this.isTouchingGround; }
    }

    private void Update()
    {
        if (!this.isTouchingGround)
            this.invokeOnLand = true;

        if (this.isTouchingGround && this.invokeOnLand)
        {
            this.OnLand?.Invoke();
            this.invokeOnLand = false;
        }
    }

    private void FixedUpdate()
    {
        this.isTouchingGround = Physics2D.OverlapCircle(this.groundCheck.position, this.groundCheckRadius, this.groundLayer);
    }
}
