using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher launcher;

    [SerializeField] InputField roomNameInputField;
    [SerializeField] Text error;
    [SerializeField] Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] Text charChoosing;

    private void Awake()
    {
        PlayerPrefs.SetString("Skin", "Player"); charChoosing.text = "You're Choosing Default";
        launcher = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // Step 1
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        // Step 2
        Debug.Log("Connected Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // Step 3
        Debug.Log("Joined Lobby");
        MenuManager.menuManager.OpenMenu("title");
        PhotonNetwork.NickName = "Nguoi choi " + Random.Range(0, 1000).ToString("0000");
    }

    public void CreatRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.menuManager.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.menuManager.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        error.text = "Room Creat Failed " + message;
        MenuManager.menuManager.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.menuManager.OpenMenu("loading");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnLeftRoom()
    {
        MenuManager.menuManager.OpenMenu("title");
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        MenuManager.menuManager.OpenMenu("loading");

        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void OnChar0Click()
    {
        PlayerPrefs.SetString("Skin", "Player");
        charChoosing.text = "You're Choosing Default";
    }
    public void OnChar1Click()
    {
        PlayerPrefs.SetString("Skin", "Player1");
        charChoosing.text = "You're Choosing Basketball";
    }
    public void OnChar2Click()
    {
        PlayerPrefs.SetString("Skin", "Player2");
        charChoosing.text = "You're Choosing Tennis";
    }
    public void OnChar3Click()
    {
        PlayerPrefs.SetString("Skin", "Player3");
        charChoosing.text = "You're Choosing Fireball";
    }

    public void QuitGame()
    {
        Application.Quit();
    }   
}
