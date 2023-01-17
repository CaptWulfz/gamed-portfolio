using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDashable
{
    float dashForce { get; set; }
    bool canDash { get; set; }
    bool isDashing { get; set; }

    void Dash();

    IEnumerator DashWait();
}
