using System;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;

    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] public InputActionReference shoot;
    [SerializeField] public InputActionReference move;
    [SerializeField] public InputActionReference look;
    [SerializeField] public InputActionReference jump;

    [SerializeField] public float gravityScale = 1f;
    [SerializeField] public float gravityForce = 9.81f;
    [SerializeField] public float cameraClampPos = 89f;
    [SerializeField] public float cameraClampNeg = -89;
    [SerializeField] public float cameraSensitivity = 20f;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerTransform; // because I wanted to seperate rb rotation from camera rotation
    [SerializeField] private Transform gunBarrelRoot;

    private Rigidbody rb_Body;

    //SmoothDamping
    private Vector3 refVector = Vector3.zero;

    //Directions
    private Vector2 cameraDir;
    private Vector2 bodyDir;
    private Vector2 moveDir;

    //Rotations
    [SerializeField] private float smoothTime = 25f;
    private Vector3 eulerAngleRotation;
    private Vector3 smoothRotation;

    //Velocities
    private Vector3 currentVel;
    private Vector3 targetVel;
    private Vector3 velocityChange;



    private void OnEnable()
    {
        //shoot.action.started += OnShoot;
        jump.action.started += OnJump;
        look.action.started += OnLook;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        UpdateBodyRotation();
        UpdateCameraRotation();
    }

    private void Start()
    {
        rb_Body = GetComponent<Rigidbody>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        rb_Body.AddForce(new Vector3(0, 1 * jumpForce, 0));
    }

    //private void OnShoot(InputAction.CallbackContext context)
    //{
    //    throw new NotImplementedException();
    //}

    private void MovePlayer()
    {
        moveDir = move.action.ReadValue<Vector2>();

        currentVel = rb_Body.linearVelocity;
        targetVel = new Vector3(moveDir.x, 0f, moveDir.y);
        targetVel *= maxSpeed;

        targetVel = playerTransform.TransformDirection(targetVel);

        velocityChange = targetVel - currentVel;
        velocityChange.y = 0f;
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxSpeed);

        rb_Body.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    public void UpdateBodyRotation()
    {
        bodyDir = look.action.ReadValue<Vector2>();
        
        eulerAngleRotation.y += (bodyDir.x  * cameraSensitivity * Time.deltaTime);

        smoothRotation.y = Mathf.Lerp(smoothRotation.y, eulerAngleRotation.y, smoothTime * Time.deltaTime);
    }

    public void UpdateCameraRotation()
    {
        cameraDir = look.action.ReadValue<Vector2>();

        eulerAngleRotation.x += -cameraDir.y * cameraSensitivity * Time.deltaTime;
        eulerAngleRotation.x = Mathf.Clamp(eulerAngleRotation.x, -89f, 90f);

        smoothRotation.x = Mathf.Lerp(smoothRotation.x, eulerAngleRotation.x, smoothTime * Time.deltaTime);
    }


    void FixedUpdate()
    {
        MovePlayer();
    }

    private void LateUpdate()
    {

        cameraTransform.eulerAngles = smoothRotation;
        rb_Body.rotation = Quaternion.Euler(0f,smoothRotation.y,0f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(gunBarrelRoot.position, gunBarrelRoot.forward);
    }
}
