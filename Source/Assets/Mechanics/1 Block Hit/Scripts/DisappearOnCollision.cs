using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearOnCollision : MonoBehaviour
{
    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("Ball"))
        {
            Destroy(gameObject);
        }
    }
}
