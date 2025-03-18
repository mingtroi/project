using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer; 

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;
    private Transform[] waypoints;

    private float baseSpeed;
    public void SetPath(Transform[] selectedPath)
    {
        waypoints = selectedPath;
        pathIndex = 0;
        baseSpeed = moveSpeed;
        target = waypoints[pathIndex];
    }

    void Update()
    {
        if (target == null || waypoints == null) return;

        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex == waypoints.Length)
            {
                LevelManager.main.TakeDamage(1);
                LevelManager.main.IncreaseKillCount();
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            target = waypoints[pathIndex];
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        
        if (direction.x != 0) 
        {
            spriteRenderer.flipX = direction.x > 0;
            
        }

    }
    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }
}
