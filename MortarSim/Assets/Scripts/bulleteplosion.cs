using System.Collections.Generic;
using UnityEngine;

public class bulleteplosion : MonoBehaviour
{
    [Header("Ground Detection (Capsule Collider)")]
    public string groundTag = "Ground"; // tag your ground objects with this
    public GameObject explosionEffect;  // optional particle effect prefab

    [Header("Explosion Detection (Sphere Collider - set as Trigger)")]
    public float explosionRadius = 5f; // should match your sphere collider radius
    public LayerMask destroyableLayers; // set this to whatever layers should be destroyed

    private List<GameObject> objectsInRange = new List<GameObject>();
    private bool hasExploded = false;

    // Call this from your shooting script right after Instantiate to ignore collision with the shooter/mortar
    public void IgnoreCollisionWith(Collider other)
    {
        Collider myCollider = GetComponent<Collider>();
        if (myCollider != null && other != null)
        {
            Physics.IgnoreCollision(myCollider, other);
        }
    }

    // Called by the physical (non-trigger) Capsule Collider when it physically hits something
    private void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        // Explode on touching ground (or really anything solid, if you remove the tag check)
        if (collision.gameObject.CompareTag(groundTag))
        {
            Explode();
        }
    }

    // Called by the Sphere Collider (must be set to "Is Trigger" in the Inspector)
    private void OnTriggerEnter(Collider other)
    {
        if (hasExploded) return;

        // Only track objects on the layers you want destroyed
        if (((1 << other.gameObject.layer) & destroyableLayers) != 0)
        {
            if (!objectsInRange.Contains(other.gameObject))
                objectsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInRange.Contains(other.gameObject))
            objectsInRange.Remove(other.gameObject);
    }

    private void Explode()
    {
        hasExploded = true;

        // Spawn explosion effect if assigned
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Destroy everything currently detected inside the sphere collider
        foreach (GameObject obj in objectsInRange)
        {
            if (obj != null)
                Destroy(obj);
        }

        objectsInRange.Clear();

        // Destroy the bullet itself
        Destroy(gameObject);
    }
}

