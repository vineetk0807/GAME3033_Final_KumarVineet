using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private List<ParticleCollisionEvent> collisionEvents;

    private float timer = 0f;

    private bool isGoForDestroy = false;
    private float destroyTimer = 0f;

    [Header("Slow motion speed")] 
    public float slowMotionSimulationSpeed = 0.01f;

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
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<EnemyController>().DestroyEnemy();
                isGoForDestroy = true;
                destroyTimer = timer;
            }
        }
    }

    // Destroy on fixed update
    private void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            timer += Time.deltaTime * GameManager.GetInstance().timeScaleFactor;

            if (timer > 4f)
            {
                Destroy(gameObject);
            }

            // if collided with something
            if(isGoForDestroy)
            {
                if (timer - destroyTimer >= 0.2f)
                {
                    Destroy(gameObject);
                }
            }            
        }

        if (GameManager.GetInstance().isTimeSlowed)
        {
            var particleMainSettings = particleSystem.main;
            particleMainSettings.simulationSpeed = slowMotionSimulationSpeed;
        }
        else
        {
            var particleMainSettings = particleSystem.main;
            particleMainSettings.simulationSpeed = 1;
        }
    }
}
