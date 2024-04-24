using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGround : MonoBehaviour
{
    Transform respawnPoint;
    void Start()
    {
        respawnPoint = GameObject.Find("RespawnPoint").transform;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            
            col.transform.position = respawnPoint.position;
        }
    }
}
