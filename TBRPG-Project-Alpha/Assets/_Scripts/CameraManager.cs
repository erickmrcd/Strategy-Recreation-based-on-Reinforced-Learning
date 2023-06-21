using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ona : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraManager;
    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionCompleted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted1; ; 
    }

    /// <summary>
    /// Bases the action_ on any action completed1.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void BaseAction_OnAnyActionCompleted1(object sender, System.EventArgs e)
    {
        switch (sender)
        {
            case MoveAction moveAction:
                Unit unit = moveAction.GetUnit();
                actionCameraManager.GetComponent<CinemachineVirtualCamera>().Follow = unit.GetUnitWorldPosition();
                actionCameraManager.GetComponent<CinemachineVirtualCamera>().LookAt = unit.GetUnitWorldPosition();
                ShowActionCamera();
                break;
            case MeleeAction meleeAction:
                Unit unitMelee = meleeAction.GetUnit();
                actionCameraManager.GetComponent<CinemachineVirtualCamera>().Follow = unitMelee.GetUnitWorldPosition();
                actionCameraManager.GetComponent<CinemachineVirtualCamera>().LookAt = unitMelee.GetUnitWorldPosition();
                ShowActionCamera();
                break;
            case BowAction bowAction:
                Unit unitBow = bowAction.GetUnit();
                actionCameraManager.GetComponent<CinemachineVirtualCamera>().Follow = unitBow.GetUnitWorldPosition();
                actionCameraManager.GetComponent<CinemachineVirtualCamera>().LookAt = unitBow.GetUnitWorldPosition();
                ShowActionCamera();
                break;
            case HealAction healAction:
                Unit unitHeal = healAction.GetUnit();
                actionCameraManager.GetComponent<CinemachineVirtualCamera>().Follow = unitHeal.GetUnitWorldPosition();
                actionCameraManager.GetComponent<CinemachineVirtualCamera>().LookAt = unitHeal.GetUnitWorldPosition();
                ShowActionCamera();
                break;
            case ShootAction shootAction:
                Unit unitShoot = shootAction.GetUnit();
                actionCameraManager.GetComponent<CinemachineVirtualCamera>().Follow = unitShoot.GetUnitWorldPosition();
                actionCameraManager.GetComponent<CinemachineVirtualCamera>().LookAt = unitShoot.GetUnitWorldPosition();
                ShowActionCamera();
                break;
            case FireBallAction fireballAction:
                Unit unitFireball = fireballAction.GetUnit();
                actionCameraManager.GetComponent<CinemachineVirtualCamera>().Follow = unitFireball.GetUnitWorldPosition();
                actionCameraManager.GetComponent<CinemachineVirtualCamera>().LookAt = unitFireball.GetUnitWorldPosition();
                ShowActionCamera();
                break;

        }
    }

    /// <summary>
    /// Bases the action_ on any action complaeted.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void BaseAction_OnAnyActionCompleted(object sender, System.EventArgs e)
    {
        HideActionCamera();
    }

    /// <summary>
    /// Shows the action camera.
    /// </summary>
    private void ShowActionCamera()
    {
        actionCameraManager.SetActive(true);
    }

    /// <summary>
    /// Hides the action camera.
    /// </summary>
    private void HideActionCamera()
    {
        actionCameraManager.SetActive(false);
    }
}
