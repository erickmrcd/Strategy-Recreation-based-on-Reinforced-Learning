using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public static event EventHandler OnAnyFireBallExploded;

    private Vector3 targetPosition;
    private Action OnFireBallBehaviourComplete;

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float moveSpeed = 15f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(transform.position, targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }
            }

            OnAnyFireBallExploded?.Invoke(this, EventArgs.Empty);

            Destroy(gameObject);

            OnFireBallBehaviourComplete();
        }
    }


    public void Setup(GridPosition targetGridPosition, Action onFireBallBehaviourComplete)
    {
        this.OnFireBallBehaviourComplete = onFireBallBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }

}
