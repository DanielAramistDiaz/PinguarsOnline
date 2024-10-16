using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;
public class Weapon : MonoBehaviour
{
    [Header("General Settings")]
    public Camera camera;
    public int damage;
    public float fireRate;
    public bool isAutomatic;
    public bool useAmmo = true;
    public float range = 100f;
    public GameObject[] weaponMesh;
    public bool isInHand;
    public AudioSource audioExit;

    [Header("VFX")]

    public GameObject hitVFX;
    private float nextFire;

    [Header("Ammo")]
    public int mag = 6;

    public int ammo = 30;
    public int magAmmo = 6;

    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    [Header("Animation")]

    public Animation animation;
    public AnimationClip reload;


    [Header("Recoil Settings")]
    
    //[Range(0,1)]    
    //public float recoilPercent = 0.3f;

    [Range(0, 2)]
    public float recoverPercent = 0.7f;
    [Space]
    public float recoilUP = 1f;

    public float recoilBack = 0f;

    public Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;

    private float recoilLength;
    private float recoverLenght;

    private bool recoiling;
    public bool recovering;
    private void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;

        originalPosition = transform.localPosition;

        recoilLength = 0;
        recoverLenght = 1 / fireRate * recoverPercent;
    }
    public void ResetPosition() 
    {
        originalPosition = transform.position;
    }    
    void Update()
    {
        //FireRate
        {
            if (nextFire > 0)
            {
                nextFire -= Time.deltaTime;
            }
        }

        //FireMode
        {
            if (isAutomatic == false)
            {
                if (Input.GetButtonDown("Fire1") && nextFire <= 0 && ammo > 0 && animation.isPlaying == false)
                {
                    nextFire = 1 / fireRate;

                    if (useAmmo == true)
                    {
                        ammo--;
                    }

                    magText.text = mag.ToString();
                    ammoText.text = ammo + "/" + magAmmo;

                    Fire();
                }
            }

            if (isAutomatic == true)
            {
                if (Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0 && animation.isPlaying == false)
                {
                    nextFire = 1 / fireRate;
                    if (useAmmo == true)
                    {
                        ammo--;
                    }

                    magText.text = mag.ToString();
                    ammoText.text = ammo + "/" + magAmmo;

                    Fire();
                }
            }
        }

       

        //Reload
        {
            if (Input.GetKeyDown(KeyCode.R) && mag > 0 && animation.isPlaying == false && ammo != magAmmo)
            {
                Reload();
            }
        }

        //Recoil And Recover
        {
            if (recoiling)
            {
                Recoil();
            }

            if (recovering)
            {
                Recovering();
            }
        }
    }
    void Reload()
    {
        animation.Play(reload.name);

        if (mag > 0)
        {
            mag--;

            ammo = magAmmo;
        }

        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }
    void Fire()
    {

        audioExit.Play();

        recoiling = true;
        recovering = false;

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        

        RaycastHit hit;

        if (Physics.Raycast(ray.origin,ray.direction,out hit, range))
        {
            PhotonNetwork.Instantiate(hitVFX.name,hit.point,Quaternion.identity);

            if (hit.transform.gameObject.GetComponent<Health>() && hit.transform.gameObject.GetComponent<Health>().health > 1)
            {
                //PhotonNetwork.LocalPlayer.AddScore(damage);
                
                if (damage >= hit.transform.gameObject.GetComponent<Health>().health && hit.transform.gameObject.GetComponent<Health>().health > 1)
                {
                    //Kill
                    Debug.Log(hit.transform.gameObject.GetComponent<Health>().health);
                    hit.transform.gameObject.GetComponent<Health>().health = 100;

                    hit.transform.gameObject.SetActive(false);                    
                    RoomManager.instance.kills++;
                    //PhotonNetwork.LocalPlayer.AddScore(100);
                    RoomManager.instance.SetHashes();
                    hit.transform.gameObject.SetActive(true);
                }

                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
    }
    public void Recoil()
    {
        Vector3 finalPosition = new Vector3(originalPosition.x, originalPosition.y + recoilUP, originalPosition.z - recoilBack);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = true;
        }
    }
    public void Recovering()
    {
        Vector3 finalPosition = originalPosition;

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLenght);
       


        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = false;
        }
    }
    public void UpdateHud()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }
}
