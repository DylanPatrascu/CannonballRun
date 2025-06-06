using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public float stopDistance;
    public Vector3 destination;
    public bool reachedDestination;
    public float slowmoTimeScale = 1f;

    private Vector3 velocity;     // Tracks the NPC's current velocity
    private Vector3 lastPosition; // Tracks the NPC's last position
    public int health;
    public int maxHealth = 100;
    private ParticleSystem explosion;

    private void Start()
    {
        health = maxHealth;
        explosion = GetComponent<ParticleSystem>();
        explosion.Pause();
    }
    private void Update()
    {
        Vector3 destinationDirection = destination - transform.position;
        destinationDirection.y = 0; // Keep movement on the X-Z plane
        float destinationDistance = destinationDirection.magnitude;

        if (destinationDistance >= stopDistance)
        {
            reachedDestination = false;
            Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * slowmoTimeScale);
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime * slowmoTimeScale);
        }
        else
        {
            reachedDestination = true;
        }

        // Calculate velocity
        velocity = (transform.position - lastPosition) / Time.deltaTime * slowmoTimeScale;
        velocity.y = 0;
        var velocityMagnitude = velocity.magnitude;
        velocity = velocity.normalized;

        // Dot product calculations
        var fwdDotProduct = Vector3.Dot(transform.forward, velocity);
        var rightDotProduct = Vector3.Dot(transform.right, velocity);

        // Update last position
        lastPosition = transform.position;
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
    }

    public void Die()
    {
        StaticData.scrap += 5;
        explosion.Play();

        // Hide all meshes
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.enabled = false;
        }
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;

        // Wait for full explosion time before destroying
        float totalLifetime = explosion.main.duration + explosion.main.startLifetime.constantMax;
        Destroy(gameObject, totalLifetime);
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <=0)
        {
            Die();
        }
    }
}
 