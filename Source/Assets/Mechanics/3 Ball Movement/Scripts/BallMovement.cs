using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public Rigidbody2D rigidbody2d;

    private Vector2 currentVelocity = new Vector2(10, 10);
    
    void Start()
    {
        rigidbody2d.velocity = currentVelocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var newDirection = other.contacts[0].normal;
        currentVelocity = Vector2.Reflect(currentVelocity, newDirection);
        rigidbody2d.velocity = currentVelocity;
    }
}
