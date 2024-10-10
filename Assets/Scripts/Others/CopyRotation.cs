using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotation : MonoBehaviour
{
    public GameObject camera;
    
    // Update is called once per frame
    void Update()
    {
        if (camera.activeSelf)
        {
            gameObject.SetActive(false);

        }
        
    }
}
