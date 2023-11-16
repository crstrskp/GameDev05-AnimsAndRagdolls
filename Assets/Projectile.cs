using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10;

    void Awake()
    {
        Destroy(gameObject, 3);
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
    }
}
