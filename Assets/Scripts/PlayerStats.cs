using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public float moneyBalance;
    public TextMeshProUGUI moneyTextElement;

    void Start()
    {
        moneyBalance = 0;
    }

    void Update()
    {
        moneyTextElement.text = "$" + moneyBalance.ToString();
    }
}
