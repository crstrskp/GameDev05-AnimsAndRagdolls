using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
    This is not a pretty script. 

    The OverLapSphere every fixedUpdate is one thing - this should really be at smaller intervals instead. 
    Another thing is that every collider (i.e. every bodypart) will attempt to get the RagdollHandler and the NavMeshAgent of the parent - this is redundant! 
*/
public class Mine : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float blastRadius = 5f;
    [SerializeField] private float proximityRadius = 1.5f;
    [SerializeField] private float explosionForce = 10005f;
    public LayerMask targetLayerMask = new LayerMask();

    void FixedUpdate() 
    {
        var colliders = Physics.OverlapSphere(transform.position, proximityRadius, targetLayerMask);
        
        if (colliders.Length > 0)
        {
            Explode();
        }
    }

    void Explode() 
    {
        var colliders = Physics.OverlapSphere(transform.position, blastRadius, targetLayerMask);

        if (colliders.Length == 0) return;

            // Spawn and destroy explosion after 3 seconds
            Destroy(Instantiate(explosionPrefab, transform.position, Quaternion.identity), 3.0f);

            foreach (Collider col in colliders) 
            {
                var ragdoll = col.GetComponentInParent<RagdollHandler>();
                if (ragdoll) 
                    ragdoll.GoRagdoll(true);

                var navMeshAgent = col.GetComponentInParent<NavMeshAgent>();
                if (navMeshAgent)
                    navMeshAgent.isStopped = true;

                var rigidbody = col.GetComponent<Rigidbody>();

                if (rigidbody == null) return; 

                Vector3 dir = col.transform.position - transform.position;

                rigidbody.AddForce(dir * explosionForce, ForceMode.Impulse);
            }

            // Destroy the mine gameobject
            Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, proximityRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
