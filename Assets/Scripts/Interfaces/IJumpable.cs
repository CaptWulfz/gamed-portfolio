using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJumpable
{
    float jumpForce { get; set; }
    bool isJumping { get; set; }
    bool canDoubleJump { get; set; }
    bool hasDoubleJumped { get; set; }
    void Jump();
}
