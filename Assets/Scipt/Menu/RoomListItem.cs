using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] Text textRLI;
    RoomInfo roomInfo;

    public void SetUp(RoomInfo _roomInfo)
    {
        roomInfo = _roomInfo;
        textRLI.text = _roomInfo.Name;
    }

    public void OnClick()
    {
        Launcher.launcher.JoinRoom(roomInfo);
    }
}
