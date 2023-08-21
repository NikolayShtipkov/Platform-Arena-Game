using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header ("Enemy")]
    [SerializeField] private Transform enemy;

    [Header ("Movement parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float idleDuration;

    private float idleTimer;
    private Vector3 initialScale;
    private bool moveLeft;

    [Header ("Animator")]
    [SerializeField]private Animator anim;

    private void Awake()
    {
        initialScale = enemy.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("isMoving", false);
    }

    private void Update()
    {
        if (moveLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else 
                ChangeDirection();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                ChangeDirection();
        }
    }

    private void MoveInDirection(int direction)
    {
        idleTimer = 0;

        anim.SetBool("isMoving", true);

        //Face direction
        enemy.localScale = new Vector3(Mathf.Abs(initialScale.x) * direction,
            initialScale.y, initialScale.z);

        //Move in direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed, 
            enemy.position.y, enemy.position.z);
    }

    private void ChangeDirection()
    {
        anim.SetBool("isMoving", false);

        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration)
        {
            moveLeft = !moveLeft;
        }
    }
}
