using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;
    public Vector3 offset = new Vector3(0, 1.05f, 0);

    void Start()
    {
    }
    void Update()
    {
        healthBarSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.parent.position + offset);
    }
    public void SetMaxHealth(int health)
    {
        healthBarSlider.maxValue = health;
        healthBarSlider.value = health;
    }
    public void SetHealth(int health)
    {
        healthBarSlider.value = health;
    }
}
