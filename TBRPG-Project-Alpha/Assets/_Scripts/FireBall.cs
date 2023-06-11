using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public static event EventHandler OnAnyFireBallExploded;

    [SerializeField] private Transform fireballExplodedVfxPrefab;
    [SerializeField] private AnimationCurve fireballExplodedVfxCurve;

    private Vector3 targetPosition;
    private Action OnFireBallBehaviourComplete;
    private float totalDistance;
    private Vector3 positionXZ;
    private float damageRadius = 4f;

    public float DamageRadius { get => damageRadius; set => damageRadius = value; }

    private void Update()
    {
        Vector3 moveDir = (targetPosition - positionXZ).normalized;

        float moveSpeed = 15f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 4f;
        float positionY = fireballExplodedVfxCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);



        float reachedTargetDistance = .2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);
            int damage = RollDice() + RollDice() + RollDice();
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(damage);
                }
            }

            OnAnyFireBallExploded?.Invoke(this, EventArgs.Empty);
            Instantiate(fireballExplodedVfxPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);

            OnFireBallBehaviourComplete();
        }
    }

    private int RollDice()
    {
        // Esto generará un número aleatorio entre 1 y 6
        return UnityEngine.Random.Range(1, 7);
    }


    public void Setup(GridPosition targetGridPosition, Action onFireBallBehaviourComplete)
    {
        this.OnFireBallBehaviourComplete = onFireBallBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);

    }

}
