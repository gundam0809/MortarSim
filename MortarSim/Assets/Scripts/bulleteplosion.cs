using System.Collections.Generic;
using UnityEngine;

public class bulleteplosion : MonoBehaviour
{
    [Header("Arc Rotation")]
    public bool rotateToVelocity = true;
    private Rigidbody rb;

    [Header("Ground Detection (Capsule Collider - set as Trigger)")]
    public string groundTag = "Ground";
    public GameObject explosionEffect;

    [Header("Destroy Detection (assign Sphere Collider #1 here - set as Trigger)")]
    public SphereCollider destroySphere;
    public LayerMask destroyableLayers;

    [Header("Explosion Force (assign Sphere Collider #2 here - set as Trigger)")]
    public SphereCollider explosionSphere;
    public float explosionForce = 700f;
    public float upwardModifier = 0.5f;

    [Header("Spawn Safety Delay")]
    public float armDelay = 3f;

    private List<GameObject> objectsToDestroy = new List<GameObject>();
    private bool hasExploded = false;
    private bool isArmed = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Invoke(nameof(Arm), armDelay);
    }

    private void Arm()
    {
        isArmed = true;
    }

    private void Update()
    {
        if (rotateToVelocity && rb != null && rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(rb.linearVelocity.normalized);
        }
    }

    // All colliders are now triggers, so ground detection happens here too
    private void OnTriggerEnter(Collider other)
    {
        if (!isArmed || hasExploded) return;

        // Ground contact
        if (other.CompareTag(groundTag))
        {
            Explode();
            return;
        }

        // Destroyable object contact
        if (((1 << other.gameObject.layer) & destroyableLayers) != 0)
        {
            if (!objectsToDestroy.Contains(other.gameObject))
                objectsToDestroy.Add(other.gameObject);

            Explode();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsToDestroy.Contains(other.gameObject))
            objectsToDestroy.Remove(other.gameObject);
    }

    private void Explode()
    {
        hasExploded = true;

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
                Destroy(obj);
        }
        objectsToDestroy.Clear();

        float radius = explosionSphere != null ? explosionSphere.radius * transform.lossyScale.x : 8f;

        Collider[] nearby = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in nearby)
        {
            Rigidbody otherRb = col.GetComponent<Rigidbody>();
            if (otherRb != null)
                otherRb.AddExplosionForce(explosionForce, transform.position, radius, upwardModifier, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }
}