using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePaddle : MonoBehaviour
{
    public Rigidbody2D rigid;

    private const float Speed = 20;

    private static readonly Vector2 Left =  new Vector2(-Speed, 0);
    
    private static readonly Vector2 Right =  new Vector2(Speed, 0);
    
    private static readonly Vector2 Stop =  new Vector2(0, 0);

    private Vector2 _currentMovement = Stop;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _currentMovement = Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _currentMovement = Right;
        }
        else
        {
            _currentMovement = Stop;
        }
    }

    void FixedUpdate()
    {
        rigid.AddForce(_currentMovement);
    }
}
