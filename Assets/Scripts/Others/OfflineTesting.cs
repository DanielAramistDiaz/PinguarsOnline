using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineTesting : MonoBehaviour
{



    public GameObject player;
    [Space]
    public Transform[] spawnPoints;

    [Space]
    public GameObject roomCam;

    [Space]
    public GameObject nameUI;

    //[HideInInspector]
    public int kills = 0;
    //[HideInInspector]
    public int deaths = 0;




    private void Start()
    {
        nameUI.SetActive(false);

        Invoke("InvokePlayer", 0);
    }


    public void InvokePlayer()
    {
        roomCam.SetActive(false);

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {

        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0,spawnPoints.Length)];



        GameObject _player = Instantiate(player, spawnPoint.position, Quaternion.identity);

        _player.GetComponent<Health>().isLocalPlayer = true;

        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
    }

}
