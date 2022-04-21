using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private List<ParticleCollisionEvent> collisionEvents;

    private float timer = 0f;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        particleSystem.Play();
    }

    /// <summary>
    /// Particle Collision
    /// </summary>
    /// <param name="other"></param>
    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particleSystem.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Debug.Log(other.gameObject.name);
        }
    }

    // Destroy on fixed update
    private void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            timer += Time.deltaTime;

            if (timer > 4f)
            {
                Destroy(gameObject);
            }
        }
    }
}
