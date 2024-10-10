using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSwitcher : MonoBehaviour
{

    public Animation _animation;
    public AnimationClip draw;
    public GameObject currentWeapon;
    public PlayerSetup playerSetup;
    [HideInInspector] public Camera camera;
    [HideInInspector] public TextMeshProUGUI magText;
    [HideInInspector] public TextMeshProUGUI ammoText;

    private int selectedWeapon = 0;
    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previusSelectedWeapon = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeapon = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedWeapon = 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedWeapon = 4;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectedWeapon = 5;
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectedWeapon = 6;
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            selectedWeapon = 7;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            selectedWeapon = 8;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }

            else
            {
                selectedWeapon += 1;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }

            else
            {
                selectedWeapon -= 1;
            }
        }
        if (previusSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropWeapon();
        }
    }

    void SelectWeapon()
    {
        if (selectedWeapon >= transform.childCount)
        {
            selectedWeapon = transform.childCount - 1;
        }
        _animation.Stop();
        _animation.Play(draw.name);

        int i = 0;

        foreach (Transform _weapon in transform)
        {
            if (i == selectedWeapon)
            {
                _weapon.gameObject.SetActive(true);
                currentWeapon = _weapon.gameObject;
            }
            else
            {
                _weapon.gameObject.SetActive(false);
            }

            i++;
        }

        if (currentWeapon.name != "Melee")
        {
        currentWeapon.GetComponent<Weapon>().UpdateHud();
        currentWeapon.GetComponent<Animation>().Play();
        }
        else
        {
        currentWeapon.GetComponent<Weapon>().UpdateHud();
        }
    }

    public void DropWeapon()
    {
        //currentWeapon.GetComponent<Weapon>().ResetPosition();
        //currentWeapon.GetComponent<Sway>().ResetPosition();

        if (currentWeapon.name != "Melee")
        {
            if (currentWeapon.GetComponent<Weapon>() == true)
            {
                playerSetup.nextWeaponGrab = 0;
                currentWeapon.GetComponent<Animation>().Stop();

                Weapon wepon = currentWeapon.GetComponent<Weapon>();
                wepon.enabled = false;

                Sway sway = currentWeapon.GetComponent<Sway>();
                sway.enabled = false;

                currentWeapon.transform.SetParent(null);

                currentWeapon.AddComponent<Rigidbody>();
                currentWeapon.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                currentWeapon.GetComponent<Rigidbody>().useGravity = true;
                currentWeapon.GetComponent<Rigidbody>().AddForce((transform.forward + transform.up) * 300f);
                currentWeapon.GetComponent<Rigidbody>().AddTorque(new Vector3(0, -1000f, 0));

                currentWeapon.GetComponent<MeshCollider>().isTrigger = false;

                //currentWeapon.layer = LayerMask.NameToLayer("Default");

                for (int i = 0; i < currentWeapon.GetComponent<Weapon>().weaponMesh.Length; i++)
                {
                    currentWeapon.GetComponent<Weapon>().weaponMesh[i].layer = LayerMask.NameToLayer("Default");
                }

                currentWeapon.tag = "GrabbableWeapon";

                //Invoke("SelectWeapon",0.1f);
                SelectWeapon();

                currentWeapon.GetComponent<Weapon>().UpdateHud();
                //Invoke("ChangeWeaponOnDrop", 3f);

            }
        }
        else
        {
            Debug.Log("ManoVacia");
        }
        /*
        Weapon wepon = currentWeapon.GetComponent<Weapon>();
        wepon.enabled = false;

        Sway sway = currentWeapon.GetComponent<Sway>();
        sway.enabled = false;

        currentWeapon.GetComponent<Sway>().ResetPosition();


        currentWeapon.transform.SetParent(null);
        selectedWeapon += 1;*/
    }

   
}
