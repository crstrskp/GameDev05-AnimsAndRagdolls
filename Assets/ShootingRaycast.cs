using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRaycast : MonoBehaviour
{


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            
            // ray from center of screen
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            if (Physics.Raycast(ray, out hit))
            {
                // Debug.Log(hit.transform.name);
                if (hit.transform.tag == "Enemy")
                {
                    //Destroy(hit.transform.gameObject);
                    hit.transform.GetComponent<Renderer>().material.color = Color.black;
                }
            }
        }
    }
}
