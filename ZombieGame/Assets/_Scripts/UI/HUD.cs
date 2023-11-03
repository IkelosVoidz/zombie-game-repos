using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class HUD : MonoBehaviour
{
    private TMP_Text bulletText, healthText;


    // Start is called before the first frame update
    void Start()
    {
        bulletText = GameObject.Find("Bullet/BulletText").GetComponent<TMPro.TextMeshProUGUI>();
        healthText = GameObject.Find("Health/HealthText").GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void onHealthChanged(int newHealth, Vector3 attackDirection)
    {
        healthText.text = $"Health: {newHealth}";
    }

    public void changeBulletText(int actualMagazine, int totalMagazine)
    {
        bulletText.text = $"{actualMagazine} / {totalMagazine}";
    }
}
