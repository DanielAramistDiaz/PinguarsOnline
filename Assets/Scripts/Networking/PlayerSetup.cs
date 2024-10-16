using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    public RoomManager roomManager;

    public Movement movement;
    public GameObject camera;
    public GameObject[] playerMesh;

    public string nickname;

    public TextMeshPro nicknameText;


    public WeaponSwitcher weaponSwitcher;
    public GameObject currentWeapon;
    public float nextWeaponGrab;
    public float grabRate;
    public float grabLimit;


    private void Update()
    {
        currentWeapon = weaponSwitcher.currentWeapon;

        {
            if (nextWeaponGrab < grabLimit)
            {
                nextWeaponGrab += Time.deltaTime;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ammo" && currentWeapon.name != "Melee")
        {
            Debug.Log("GRABBABLE AMMO");
            currentWeapon.GetComponent<Weapon>().mag += 5;
            currentWeapon.GetComponent<Weapon>().UpdateHud();
            other.gameObject.SetActive(false);
        }

        if (other.tag == "GrabbableWeapon" && nextWeaponGrab >= grabLimit )
        {
            Debug.Log("GRABBABLE WEAPON");
            
            nextWeaponGrab = 1 / grabRate;

            GameObject grabbableWeaponGO = other.gameObject;


            grabbableWeaponGO.transform.SetParent(weaponSwitcher.transform);

            grabbableWeaponGO.GetComponent<Weapon>().Recoil();
            grabbableWeaponGO.GetComponent<Weapon>().Recovering();

            grabbableWeaponGO.transform.localRotation = new Quaternion(0, 0, 0, 0);
            grabbableWeaponGO.gameObject.SetActive(false);

            Destroy(grabbableWeaponGO.GetComponent<Rigidbody>());
            grabbableWeaponGO.GetComponent<MeshCollider>().isTrigger = true;

            
            for (int i = 0; i < grabbableWeaponGO.GetComponent<Weapon>().weaponMesh.Length; i++)
            {
                grabbableWeaponGO.GetComponent<Weapon>().weaponMesh[i].layer = LayerMask.NameToLayer("Hand");
            }

            grabbableWeaponGO.GetComponent<Weapon>().camera = weaponSwitcher.camera;
            grabbableWeaponGO.GetComponent<Weapon>().magText = weaponSwitcher.magText;
            grabbableWeaponGO.GetComponent<Weapon>().ammoText = weaponSwitcher.ammoText;

            grabbableWeaponGO.GetComponent<Sway>().enabled = true;

            currentWeapon.tag = "Untagged";

            grabbableWeaponGO.GetComponent<Weapon>().enabled = true;
            grabbableWeaponGO.GetComponent<Animation>().Play();

            //grabbableWeaponGO.GetComponent<Weapon>().UpdateHud();

            
        }

        if (other.tag == "MasDanio")
        {
            
            currentWeapon.GetComponent<Weapon>().damage++;
            currentWeapon.GetComponent<Weapon>().UpdateHud();
            other.gameObject.SetActive(false);
        }

        if (other.tag == "MasVeloAr")
        {            
            currentWeapon.GetComponent<Weapon>().fireRate++;
            currentWeapon.GetComponent<Weapon>().UpdateHud();
            other.gameObject.SetActive(false);
        }

        if (other.tag == "MasCura")
        {
            GetComponent<Health>().cureRate++;
            other.gameObject.SetActive(false);
        }

        if (other.tag == "MasRapidez")
        {
            float oldWalkSpeed = GetComponent<Movement>().walkSpeed;
            float oldSprintSpeed = GetComponent<Movement>().sprintSpeed;

            //float newWalkSpeed = oldWalkSpeed * 0.1f + oldWalkSpeed;
            //float newSprintSpeed = oldSprintSpeed * 0.1f + oldSprintSpeed;

            float newWalkSpeed = oldWalkSpeed +2;
            float newSprintSpeed = oldSprintSpeed +2;

            GetComponent<Movement>().walkSpeed = newWalkSpeed;
            GetComponent<Movement>().sprintSpeed = newSprintSpeed;

            other.gameObject.SetActive(false);
        }
    }

    public void IsLocalPlayer()
    {
        for (int i = 0; i < playerMesh.Length; i++)
        {
        playerMesh[i].layer = LayerMask.NameToLayer("DontRemder");
        }
        movement.enabled = true;
        camera.SetActive(true);
    }

    [PunRPC]
    public void SetNickname (string _name)
    {
        nickname = _name;

        nicknameText.text = nickname;
    }
}
