using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{

    [Header("Player Reference")] 
    public Transform playerLocation;

    [Header("Navigation Mesh Agent")]
    public NavMeshAgent agent;
    public float stoppingDistance = 1;

    [Header("Animations")] 
    private Animator _enemyAnimator;
    public readonly int isFollowingHash = Animator.StringToHash("IsFollowing");
    public bool isFollowing = false;

    private CapsuleCollider _collider;
    public List<SkinnedMeshRenderer> meshRenderer;

    [Header("Shatter effect")]
    public GameObject ShatterEffectBotPrefab;
    public float shatterForce = 10f;
    private GameObject shatteredXbot = null;


    private bool isDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(playerLocation.position);
        isFollowing = true;

        _enemyAnimator.SetBool(isFollowingHash,true);

        _collider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(playerLocation.position);
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

        // deactivate mesh renderer
        foreach (var mesh in meshRenderer)
        {
            mesh.enabled = false;
        }

        // disable the main collider
        _collider.enabled = false;

        // Instantiate the shattered xbot
        shatteredXbot = Instantiate(ShatterEffectBotPrefab, transform.position, transform.rotation);

        // add 
        foreach (var rb in shatteredXbot.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = (rb.position - transform.position).normalized * shatterForce;
            rb.AddForce(force);
        }

        StartCoroutine(DestroyCoroutine());
    }

    /// <summary>
    /// Coroutine to destroy
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(shatteredXbot);
        Destroy(gameObject);
    }
}
