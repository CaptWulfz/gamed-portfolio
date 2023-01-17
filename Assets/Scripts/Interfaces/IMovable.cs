using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    bool canMove { get; set; }
    void Move(float pos);
}
