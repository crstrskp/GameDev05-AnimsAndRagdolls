using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingProjectile : MonoBehaviour
{
    public GameObject projectile;
    public Transform muzzle;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject projectile = Instantiate(this.projectile, muzzle.position, muzzle.rotation) as GameObject;
        }
    }
}
