using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private Transform proyectilePrefab;
    [SerializeField] private Transform shootPointTransform;


    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        if (TryGetComponent<BowAction>(out BowAction bowAction))
        {
            bowAction.OnBowShoot += BowAction_OnShoot; ;
        }
        if(TryGetComponent<MeleeAction>(out MeleeAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
        }
    }

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("Shoot");
    }

    private void BowAction_OnAiming(object sender, EventArgs e)
    {
        animator.SetTrigger("Shoot");
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        //Transform bulletProjectileTransform = Instantiate(proyectilePrefab, shootPointTransform.position, Quaternion.identity);

        //Projectile projectile = bulletProjectileTransform.GetComponent<Projectile>();

        //Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        //targetUnitShootAtPosition.y = shootPointTransform.position.y;

        //projectile.Setup(targetUnitShootAtPosition);

    }

    private void BowAction_OnShoot(object sender, BowAction.OnBowShootEventArgs e)
    {
       

        //animator.SetTrigger("Shoot");
        
        Transform bulletProjectileTransform = Instantiate(proyectilePrefab, shootPointTransform.position, Quaternion.Euler(90f, shootPointTransform.rotation.y, shootPointTransform.rotation.z));

        Arrow projectile = bulletProjectileTransform.GetComponent<Arrow>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        projectile.Setup(targetUnitShootAtPosition);

    }



}
