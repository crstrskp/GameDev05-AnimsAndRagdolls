using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{

    [SerializeField] private GameObject minePrefab;

    // Update is called once per frame
    void Update()
    {
         if (Input.GetMouseButtonDown(0))
        {
             RaycastHit hit;
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                Instantiate(minePrefab, hit.point, Quaternion.identity);
            }
        }
    }
}
