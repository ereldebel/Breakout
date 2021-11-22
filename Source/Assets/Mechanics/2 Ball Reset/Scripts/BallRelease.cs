using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRelease : MonoBehaviour
{
    private bool _ballIsFixed = true;

    private static Vector2 StartingPosition = new Vector3(0,0, 0);
    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyUp(KeyCode.Space)) return;
        if (_ballIsFixed)
        {
            _ballIsFixed = false;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            return;
        }
        _ballIsFixed = true;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.position = StartingPosition;
    }
}
