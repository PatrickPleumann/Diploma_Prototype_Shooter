using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] public BeatTracking beatTracker;

    [SerializeField] private float speed;
    [SerializeField] private float airSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private float dashTimer;
    private bool canDash = true;
    private Vector3 targetDir;
    [SerializeField] public float dashForce;

    [SerializeField] public InputActionReference shoot;
    [SerializeField] public InputActionReference move;
    [SerializeField] public InputActionReference look;
    [SerializeField] public InputActionReference jump;
    [SerializeField] public InputActionReference dash;

    [SerializeField] public float gravityScale = 1;
    [SerializeField] public float gravityForce = 9.81f;
    [SerializeField] public float cameraClampPos = 85f;
    [SerializeField] public float cameraClampNeg = -85;
    [SerializeField] public float cameraSensitivity = 20f;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerTransform; // because I wanted to seperate rb rotation from camera rotation
    [SerializeField] private Transform gunBarrelRoot;
    private Vector3 cameraCenterPoint;

    [SerializeField] private Vector3 groundCheckPos;
    [SerializeField] private Vector3 groundCheckSize;

    [SerializeField] public LayerMask groundCheckLayers;
    [SerializeField] public LayerMask targetLayerMask;
    [SerializeField] public LayerMask wallCheckLayers;

    [SerializeField] private Transform wallCheckRoot;
    [SerializeField] private float wallCheckRadius;

    [SerializeField] public GameObject bullet;
    [SerializeField] public Transform bulletParent;


    private Rigidbody rb_Body;
    private bool isGrounded = true;
    private Collider[] touchesWall;

    //Directions
    private float cameraDir;
    private float bodyDir;
    private Vector2 moveDir;

    //Rotations
    [SerializeField] private float smoothTime = 25f;
    private Vector3 eulerAngleRotation;
    private Vector3 smoothRotation;

    //Velocities
    private Vector3 currentVel;
    private Vector3 targetVel;
    private Vector3 velocityChange;


    //Forces
    private Vector3 jumpForceVector;
    private Vector3 wallJumpDir;
    private bool canMove = true;


    private void OnEnable()
    {
        shoot.action.started += OnShoot;
        jump.action.started += OnJump;
        dash.action.started += OnDash;

    }

    private void OnDisable()
    {
        shoot.action.started -= OnShoot;
        jump.action.started -= OnJump;
        dash.action.started -= OnDash;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb_Body = GetComponent<Rigidbody>();
        jumpForceVector = new Vector3(0f, 1, 0f);
        cameraCenterPoint = new Vector3(0.5f, 0.5f, 1.0f);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (canDash == true)
        {
            
            canDash = false;
            targetDir = playerTransform.TransformDirection(targetVel);
            rb_Body.AddForce(targetVel * rb_Body.mass * dashForce, ForceMode.Impulse);
            StartCoroutine(StartDashTimer());
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        //EventHandler.onWeaponShoot.Invoke();
        if (beatTracker.isOnBeat == true)
        {
            Debug.Log("ON BEAT!");
        }

        if (isGrounded)
            wallJumpDir = playerTransform.forward;

        if (isGrounded)
        {
            isGrounded = false;
            rb_Body.AddForce(jumpForceVector * rb_Body.mass * jumpForce, ForceMode.Impulse);
        }
        else if (!isGrounded && touchesWall.Length > 0)
        {
            if (beatTracker.isOnBeat == true)
            {
                Debug.Log("On Beat");
            }
            canMove = false;
            var cur = Vector3.Reflect(wallJumpDir, touchesWall[0].transform.forward);
            cur.y += 0.3f;
            rb_Body.AddForce(cur.normalized * rb_Body.mass * jumpForce, ForceMode.Impulse);
        }
    }
    
    private void OnShoot(InputAction.CallbackContext context)
    {
        var tempPos = Camera.main.ViewportPointToRay(cameraCenterPoint);
        var tempBullet = Instantiate(bullet, tempPos.origin, Quaternion.Euler(transform.eulerAngles));
        var tempBulletRB = tempBullet.GetComponent<Rigidbody>();
        tempBulletRB.AddForce(tempPos.direction * 20, ForceMode.Impulse);

        if (beatTracker.isOnBeat == true)
        {
            Debug.Log("ON BEAT!");
        }
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(cameraCenterPoint);
        var temp = Physics.Raycast(ray, out hit, 100f, targetLayerMask);
        if (temp == true)
        {
            Debug.Log("HIT!");
        }
    }


    private void MovePlayer()
    {
        if (canMove == true)
        {
            moveDir = move.action.ReadValue<Vector2>();

            currentVel = rb_Body.linearVelocity;
            targetVel = new Vector3(moveDir.x, 0f, moveDir.y);

            targetVel *= speed;

            targetVel = playerTransform.TransformDirection(targetVel);

            velocityChange = targetVel - currentVel;
            velocityChange.y = 0f;
            velocityChange = Vector3.ClampMagnitude(velocityChange, maxSpeed);

            rb_Body.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    public void UpdateBodyRotation()
    {
        bodyDir = look.action.ReadValue<Vector2>().x;
        eulerAngleRotation.y += (bodyDir * cameraSensitivity * Time.deltaTime);

        smoothRotation.y = Mathf.Lerp(smoothRotation.y, eulerAngleRotation.y, smoothTime * Time.deltaTime);
    }

    public void UpdateCameraRotation()
    {
        cameraDir = look.action.ReadValue<Vector2>().y;
        eulerAngleRotation.x += -cameraDir * cameraSensitivity * Time.deltaTime;
        eulerAngleRotation.x = Mathf.Clamp(eulerAngleRotation.x, cameraClampNeg, cameraClampPos);

        smoothRotation.x = Mathf.Lerp(smoothRotation.x, eulerAngleRotation.x, smoothTime * Time.deltaTime);
    }


    void FixedUpdate()
    {
        if (Physics.OverlapBox(playerTransform.position + groundCheckPos, groundCheckSize / 2, Quaternion.identity, groundCheckLayers).Length > 0)
        {
            canMove = true;
            isGrounded = true;
        }
        else
            isGrounded = false;


        touchesWall = Physics.OverlapSphere(wallCheckRoot.position, wallCheckRadius, wallCheckLayers);

        UpdateBodyRotation();
        UpdateCameraRotation();
        MovePlayer();

        rb_Body.rotation = Quaternion.Euler(0f, smoothRotation.y, 0f);
        cameraTransform.eulerAngles = smoothRotation;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(gunBarrelRoot.position, gunBarrelRoot.forward);
        Gizmos.DrawCube(playerTransform.position + groundCheckPos, groundCheckSize);
        Gizmos.DrawSphere(wallCheckRoot.position, wallCheckRadius);
    }

    IEnumerator StartDashTimer()
    {
        yield return new WaitForSeconds(3);
        canDash = true;
        yield break;
    }
}
