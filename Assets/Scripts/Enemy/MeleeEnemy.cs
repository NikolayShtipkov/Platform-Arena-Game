using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header ("Attack patameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;

    [Header ("Collider parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private AudioSource hitSoundEffect;

    private float cooldownTimer = Mathf.Infinity;
    private Animator anim;
    private PlayerLife playerLife;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInsight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                hitSoundEffect.Play();
                cooldownTimer = 0;
                anim.SetTrigger("attack");
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInsight();
        }
    }

    private bool PlayerInsight()
    {
        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerLife = hit.transform.GetComponent<PlayerLife>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    public void Die()
    {
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = false;
        }
        
        GetComponent<MeleeEnemy>().enabled = false;
        anim.SetTrigger("die");
    }

    private void KillPlayer()
    {
        if (PlayerInsight())
        {
            GetComponent<MeleeEnemy>().enabled = false;
            playerLife.Die();
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
