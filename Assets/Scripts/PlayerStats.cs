using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Range(0, 100)]public float lifeValue;
    public Slider healthBar;

    public float moneyBalance;
    public TextMeshProUGUI moneyTextElement;

    void Start()
    {
        moneyBalance = 0;
    }

    void Update()
    {
        healthBar.value = lifeValue;

        moneyTextElement.text = "$" + moneyBalance.ToString();
    }
}
