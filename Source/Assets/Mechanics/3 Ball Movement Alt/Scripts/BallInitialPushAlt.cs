using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInitialPushAlt : MonoBehaviour
{
    public Rigidbody2D rigidbody2d;
    
    void Start()
    {
        rigidbody2d.velocity = new Vector2(10,10);
    }
}
