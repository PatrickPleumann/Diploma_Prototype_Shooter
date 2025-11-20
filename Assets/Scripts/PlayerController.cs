using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;

    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference look;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference shoot;

    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float gravityForce = 9.81f;

    [SerializeField] private float cameraClampPos = 89f;
    [SerializeField] private float cameraClampNeg = -89;
    [SerializeField] private float cameraSensitivity = 20f;
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;
    private Vector2 lookDir;


    private void OnEnable()
    {
        shoot.action.started += OnShoot;
        jump.action.started += OnJump;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        rb.AddForce(new Vector3(0, 1 * jumpForce, 0));
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void MovePlayer()
    {
        var moveDirection = move.action.ReadValue<Vector2>();

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(moveDirection.x, 0f, moveDirection.y);
        targetVelocity *= maxSpeed;

        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocityChange = targetVelocity - currentVelocity;
        velocityChange.y = 0f;
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxSpeed);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    public void UpdateRotation()
    {
        var rotation = look.action.ReadValue<Vector2>();
        var currentRotation = cameraTransform.rotation.eulerAngles;
        var targetRotation = currentRotation + new Vector3(rotation.y, rotation.x, 0f);

        Mathf.Clamp(cameraTransform.rotation.x, 0, cameraClampPos);
        Mathf.Clamp(cameraTransform.rotation.y, 0, cameraClampNeg);
        cameraTransform.rotation = Quaternion.Euler(targetRotation);
    }


    void FixedUpdate()
    {
        MovePlayer();
    }

    private void LateUpdate()
    {
        UpdateRotation();
    }
}
