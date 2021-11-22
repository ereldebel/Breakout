using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    private static readonly int NumberOfHearts = 3; 
    
    private int _heartsLeft = NumberOfHearts;

    public int heartNumber;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _heartsLeft--;
            if (heartNumber == _heartsLeft)
            {
                Destroy(gameObject);
            }
        }
    }
}
