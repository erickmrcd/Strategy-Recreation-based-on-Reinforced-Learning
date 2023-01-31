using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private float stoppingDistance = 0.1f;
    [SerializeField] private float rotateSpeed = 10f;


    private Vector3 targetPosition;
   
    private void Awake()
    {
        targetPosition = transform.position;
    }
    private void Update()
    {
        unitAnimator.SetBool("IsWalking",true);

        float stoppingDistance = 0.1f;
        float rotateSpeed = 10f;
        if(Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            
            transform.forward = Vector3.Lerp(transform.forward,moveDirection,Time.deltaTime*rotateSpeed);
            
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }


       
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
