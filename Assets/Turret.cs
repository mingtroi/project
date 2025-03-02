using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Mathematics;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Animator animator;


    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;

    private Transform target;

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        
        RotationTowardTarget();
        UpdateAnimation();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
    }
    private void UpdateAnimation()
    {
        float angle = turretRotationPoint.eulerAngles.z;

        if (angle >= 45f && angle < 135f)
        {
            animator.Play("U_Idle");
        }
        else if (angle >= 135f && angle < 225f)
        {
            animator.Play("L_Idle"); 
        }
        else if (angle >= 225f && angle < 315f)
        {
            animator.Play("D_Idle");
        }
        else
        {
            animator.Play("R_Idle");
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
    private void RotationTowardTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y,
                                  target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation,
            targetRotation, rotationSpeed * Time.deltaTime);
    }
    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,
            (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }
}
