using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    public float waitTime = 5f;
    public GameObject player;
    [Space]
    public Transform[] spawnPoints;

    [Space]
    public GameObject roomCam;

    private string nickname = "unnamed";

    [Space]
    public GameObject nameUI;
    public GameObject connectingUI;

    //[HideInInspector]
    public int kills = 0;
    //[HideInInspector]
    public int deaths = 0;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeNickname(string _name)
    {
        nickname = _name;
    }

    public void JoinRoomButtonPressed()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.ConnectUsingSettings();


        nameUI.SetActive(false);
        connectingUI.SetActive(true);
                

        Invoke("InvokePlayer", waitTime);
    }
    void Start()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected to server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        PhotonNetwork.JoinOrCreateRoom("test",null,null);

        Debug.Log("Connected and in a room");

        //Invoke("InvokePlayer", waitTime);

        
    }

    public void InvokePlayer()
    {
        roomCam.SetActive(false);

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {

        PhotonNetwork.LocalPlayer.NickName = nickname;

        //Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0,spawnPoints.Length)];

        //GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);

        Transform spawnPoint = spawnPoints[0];

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);


        _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered,nickname);

        _player.GetComponent<Health>().isLocalPlayer = true;

        _player.GetComponent<PlayerSetup>().IsLocalPlayer();

    }

    public void SetHashes()
    {
        try
        {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;

            hash["deaths"] = deaths;
            hash["kills"] = kills;

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        catch
        {

            //Nodhat
        }
    }
}
