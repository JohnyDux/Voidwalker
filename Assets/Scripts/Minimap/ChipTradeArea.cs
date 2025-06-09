using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipTradeArea : MonoBehaviour
{
    public GameObject marker;
    public bool selected;
    void Start()
    {
        marker.SetActive(false);
    }

    void Update()
    {
        if(selected == true)
        {
            marker.SetActive(true);
        }
        else
        {
            marker.SetActive(false);
        }
    }
}
