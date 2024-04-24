using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    public Slider ammoBarSlider;
    public Vector3 offset = new Vector3(0, 0.9f, 0);
    void Start()
    {
        
    }
    void Update()
    {
        ammoBarSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.parent.position + offset);
    }
    public void SetMaxHealth(int ammo)
    {
        ammoBarSlider.maxValue = ammo;
        ammoBarSlider.value = ammo;
    }
    public void SetHealth(int ammo)
    {
        ammoBarSlider.value = ammo;
    }
}
