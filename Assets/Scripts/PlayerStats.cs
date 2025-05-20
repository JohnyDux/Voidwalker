using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Range(0, 100)]public float lifeValue;
    public Slider healthBar;

    [Range(0, 100)] public float oxygenValue;
    public Slider oxygenBar;

    public float moneyBalance;
    public TextMeshProUGUI moneyTextElement;

    public int bulletCount;
    public int fullMagCount;
    public TextMeshProUGUI bulletTextElement;

    void Start()
    {
        moneyBalance = 0;
        oxygenValue = 100;
    }

    void Update()
    {
        healthBar.value = lifeValue;

        oxygenBar.value = oxygenValue;

        moneyTextElement.text = "$" + moneyBalance.ToString();

        if(bulletCount > fullMagCount)
        {
            bulletCount = fullMagCount;
        }
        else if(bulletCount < 0)
        {
            bulletCount = 0;
        }

        bulletTextElement.text = bulletCount.ToString() + "/" + fullMagCount.ToString();
    }
}
