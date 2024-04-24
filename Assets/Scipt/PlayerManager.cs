using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            CreatController();
        }
    }

    void CreatController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", PlayerPrefs.GetString("Skin")),Vector3.zero,Quaternion.identity);
    }
}
