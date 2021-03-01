using UnityEngine;

public class RagdollHandler : MonoBehaviour
{
    void Start()
    {
        GoRagdoll(false);
    }

    public void GoRagdoll(bool v)
    {
        if (v == true)
        {
            // disable animator
            GetComponent<Animator>().enabled = false;
        }

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            if (rb.gameObject != gameObject)
            {
                rb.useGravity = v;
                rb.isKinematic = !v;
            }
        }
    }
}
