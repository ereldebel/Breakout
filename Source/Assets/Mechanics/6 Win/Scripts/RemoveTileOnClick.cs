using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveTileOnClick : MonoBehaviour
{
    public GameObject BG;
    void OnMouseDown()
    {
        Destroy(gameObject);
    }

    void OnDisable()
    {
        BG.GetComponent<ChangeBGColorOnWin>().removeTile();
    }
}
