using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerStatus : MonoBehaviourPunCallbacks,IPunObservable
{
    private const int maxHealth = 100;
    private int currentHealth;
    //private int MaxLife = 5;
    //private int currentLife;
    private int maxAmmo;
    private int currentAmmo;
    //Rigibody2D rb2d;
    private int damage=10;
    public HealthBar healthBar;
    public AmmoBar ammoBar;
    void Start()
    {
        //currentLife = MaxLife;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }
    
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Take " + damage);
        // KnockBack();
        
    }

    //public void Die()
    //{
        //current-=1;
    //}

    //Vector2 Dir
    void KnockBack(Transform pos)
    {
        // 
    }
    public int GetAttackDamage()
    {
        return damage;
    }
    
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
            
        }
        else if (stream.IsReading)
        {
            currentHealth = (int)  stream.ReceiveNext();
            healthBar.SetHealth(currentHealth);
        }
    }
}
