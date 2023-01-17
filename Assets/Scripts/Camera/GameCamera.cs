using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] Camera cam;

    public void Initialize()
    {
        
    }

    public void FollowTarget(Transform target)
    {
        if (target.gameObject.tag == TagNames.PLAYER)
            this.player = target;
    }
}
