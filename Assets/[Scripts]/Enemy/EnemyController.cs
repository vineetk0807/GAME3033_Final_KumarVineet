using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public enum EnemyMeshRenderer
    {
        JOINTS,
        SURFACE,
    }

    [Header("Player Reference")] 
    public Transform playerLocation;

    [Header("Navigation Mesh Agent")]
    public NavMeshAgent agent;
    public float agentSpeed = 9;

    [Header("Animations")] 
    private Animator _enemyAnimator;
    public readonly int isFollowingHash = Animator.StringToHash("IsFollowing");
    public bool isFollowing = false;

    private CapsuleCollider _collider;

    [Header("Shatter effect")]
    public GameObject ShatterEffectBotPrefab;
    public float shatterForce = 10f;
    private GameObject shatteredXbot = null;

    [Header("Materials")]
    public List<SkinnedMeshRenderer> meshRenderer;
    public List<Material> meshMaterialsTransparent;

    [Header("Slow Motion")] 
    public float slowAgentSpeed = 1f;
    public float slowAnimationRate = 0.25f;

    private bool isDestroyed = false;

    [Header("Audio")]
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;

        _enemyAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(playerLocation.position);
        isFollowing = true;

        _enemyAnimator.SetBool(isFollowingHash,true);

        _collider = GetComponent<CapsuleCollider>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isDestroyed)
        {
            return;
        }


        agent.SetDestination(playerLocation.position);


        if (GameManager.GetInstance().isTimeSlowed)
        {
            agent.speed = slowAgentSpeed;
            _enemyAnimator.speed = slowAnimationRate;
        }
        else
        {
            agent.speed = agentSpeed;
            _enemyAnimator.speed = 1f;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (gameObject)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Destroy Enemy
    /// </summary>
    public void DestroyEnemy()
    {
        if (isDestroyed)
        {
            return;
        }

        isDestroyed = true;

        agent.isStopped = true;
        _enemyAnimator.speed = 0f;

        // change material of mesh renderer
        for (int i = 0; i < meshRenderer.Count; i++)
        {
            meshRenderer[i].material = meshMaterialsTransparent[i];
        }

        // disable the main collider
        _collider.enabled = false;

        // Check if game is over
        if (!GameManager.GetInstance().isGameOver)
        {
            // Instantiate the shattered xbot
            shatteredXbot = Instantiate(ShatterEffectBotPrefab, transform.position, transform.rotation);

            // add force
            foreach (var rb in shatteredXbot.GetComponentsInChildren<Rigidbody>())
            {
                Vector3 force = (rb.position - transform.position).normalized * shatterForce;
                rb.AddForce(force);
            }

            audioSource.Play();
        }

        // Start the destroy coroutine
        StartCoroutine(DestroyCoroutine());
    }

    /// <summary>
    /// Coroutine to destroy
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyCoroutine()
    {
        GameManager.GetInstance().UpdateEnemyTakenCount();
        yield return new WaitForSeconds(2f);

        // Safety checks

        if (shatteredXbot)
        {
            Destroy(shatteredXbot);
        }

        if (gameObject)
        {
            Destroy(gameObject);
        }
    }
}
