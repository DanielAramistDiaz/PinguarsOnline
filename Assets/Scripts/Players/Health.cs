using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Health : MonoBehaviour
{
    public int health;
    public bool isLocalPlayer;
    public WeaponSwitcher weaponSwitcher;

    [Header("Settings")]
    public bool canCure;
    public float waitToCure = 0.1f;
    public float cureRate;
    public int cureInt = 1;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    [Header("Respawn Settings")]

    public float respawnTime = 5f;
    public GameObject[] disable;
    public Movement movement;

    [PunRPC]
    public void TakeDamage(int _damage)
    {
        health -= _damage;

        healthText.text = health.ToString();

        if (health <= 0)
        {
            if (isLocalPlayer)
            {
            
            /*if (isLocalPlayer)
            {
                RoomManager.instance.SpawnPlayer();
            }*/

            //gameObject.SetActive(false);


            //Transform spawnPoint = RoomManager.instance.spawnPoints[UnityEngine.Random.Range(0, RoomManager.instance.spawnPoints.Length)];
            
            healthText.text = health.ToString();
            RoomManager.instance.SetHashes();
                gameObject.SetActive(false);
                //Destroy(gameObject,10f);

                for (int i = 0; i < disable.Length; i++)
                {
                    disable[i].SetActive(false);
                }

                movement.enabled = false;
                Invoke("Respawn",respawnTime);
            }


        }
    }

    
    [PunRPC]

    public void Respawn()
    {
        for (int i = 0; i < disable.Length; i++)
        {
            disable[i].SetActive(true);
        }

        movement.enabled = true;

        if (isLocalPlayer)
        {
            

            weaponSwitcher.DropWeapon();
            RoomManager.instance.deaths++;
            Transform spawnPoint = RoomManager.instance.spawnPoints[0];
            transform.position = spawnPoint.position;
            health = 100;
            healthText.text = health.ToString();
            gameObject.SetActive(true);
            RoomManager.instance.SetHashes();
        }
    }

    [PunRPC]
    private void Update()
    {
        if (waitToCure > 0)
        {
            waitToCure -= Time.deltaTime;
        }
            AutoHealth();
    }

    private void AutoHealth()
    {
        if (waitToCure <= 0 && health < 100)
        {
            waitToCure = 1 / cureRate;

            health++;
            healthText.text = health.ToString();
        }
    }


    [ContextMenu("Respawn")]
    void CallRespawn()
    {
        Respawn();
    }

    [ContextMenu("Kill")]
    void CallKill()
    {
        TakeDamage(100);
    }

}
