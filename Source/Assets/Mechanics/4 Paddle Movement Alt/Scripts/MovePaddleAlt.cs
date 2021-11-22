using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePaddleAlt : MonoBehaviour
{

    public Rigidbody2D rigid;
    
    private const float Speed = 10;

    private static readonly Vector2 Left =  new Vector2(-Speed, 0);
    
    private static readonly Vector2 Right =  new Vector2(Speed, 0);
    
    private static readonly Vector2 Stop =  new Vector2(0, 0);
    
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigid.velocity = Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rigid.velocity = Right;
        }
        else
        {
            rigid.velocity = Stop;
        }
    }
}
