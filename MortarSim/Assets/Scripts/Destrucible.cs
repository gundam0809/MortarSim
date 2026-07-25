    using UnityEngine;

public class Destrucible : MonoBehaviour
{
    [Header("Explosion Settings")]
    public GameObject explosionEffect; // optional particle effect prefab
    public float blastRadius = 5f;     // how far the explosion reaches
    public float debrisForce = 500f;   // how hard nearby rigidbodies get pushed
    public string destructibleTag = "Destructible"; // tag all buildings/vehicles/gas-tanks with this

    public void Explode()
    {
        // Spawn explosion effect if assigned
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Find everything within the blast radius
        Collider[] nearby = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider col in nearby)
        {
            if (col.gameObject == gameObject) continue; // skip self

            // Push any nearby rigidbodies (debris, cars, etc.)
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(debrisForce, transform.position, blastRadius);

            // Chain reaction: if it's tagged as destructible, blow it up too
            if (col.CompareTag(destructibleTag))
            {
                Destroy(col.gameObject);
            }
        }

        // Destroy this object
        Destroy(gameObject);
    }
}
