using Cinemachine;
using System;
using UnityEngine;
/// <summary>
/// The camera controller.
/// </summary>

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// The m i n_ f o l l o w_ y_ o f f s e t.
    /// </summary>
    private const float MIN_FOLLOW_Y_OFFSET = 4f;
    /// <summary>
    /// The m a x_ f o l l o w_ y_ o f f s e t.
    /// </summary>
    private const float MAX_FOLLOW_Y_OFFSET = 10f;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float zoomSpeed = 25f;

    private float zoomAmount = 1f;
    private Vector3 targetFollowOffset;
    private CinemachineTransposer cinemachineTransposer;

    /// <summary>
    /// Starts the.
    /// </summary>
    private void Start()
    {
     
        cinemachineTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {

        HandleMovement();
        HandleRotation();
        HandleZoom();

    }



    /// <summary>
    /// Handles the zoom.
    /// </summary>
    private void HandleZoom()
    {
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;

        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomAmount;

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        cinemachineTransposer.m_FollowOffset =
            Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }


    /// <summary>
    /// Handles the rotation.
    /// </summary>
    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        rotationVector.y += InputManager.Instance.GetCameraRotateAmount();

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }


    /// <summary>
    /// Handles the movement.
    /// </summary>
    private void HandleMovement()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();
        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

}
