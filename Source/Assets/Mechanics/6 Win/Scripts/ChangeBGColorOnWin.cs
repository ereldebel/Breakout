using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGColorOnWin : MonoBehaviour
{
    private int tilesLeft = 9;


    public void removeTile()
    {
        tilesLeft--;
        if (tilesLeft == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
        }
    }
}
