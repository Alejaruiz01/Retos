using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float rotationSmoothTime = 0.1f;
    public float maxClimbAngle = 45f;

    [Header("Salto")]
    public float jumpForce = 5f;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    private float turnSmoothVelocity;

    // **Aquí guardamos la dirección que calculamos en Update()
    private Vector3 cachedMoveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Interpolación activada para suavizar entre pasos de física
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible     = false;
    }

    void Update()
    {
        ReadInputAndRotate();
        HandleJump();
        HandleFallingAnim();
    }

    // FixedUpdate se llama justo antes de que la física se procese
    void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ReadInputAndRotate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v).normalized;

        if (input.magnitude >= 0.1f)
        {
            // Cámara real
            Transform cam = Camera.main.transform;
            Vector3 camF = cam.forward; camF.y = 0; camF.Normalize();
            Vector3 camR = cam.right;   camR.y = 0; camR.Normalize();

            // Guardamos para usar en FixedUpdate()
            cachedMoveDir = (camF * v + camR * h).normalized;

            // Rotación suave (queda en Update para seguir el ratón)
            float targetAngle = Mathf.Atan2(cachedMoveDir.x, cachedMoveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                                                ref turnSmoothVelocity,
                                                rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            animator.SetFloat("Speed", 1f);
        }
        else
        {
            cachedMoveDir = Vector3.zero;
            animator.SetFloat("Speed", 0f);
        }
    }

    private void ApplyMovement()
    {
        Vector3 moveDir = cachedMoveDir;

        // Ajuste de pendientes (igual que antes)
        if (moveDir.magnitude > 0.1f)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f)
                && hit.collider.CompareTag("Ground"))
            {
                float slope = Vector3.Angle(hit.normal, Vector3.up);
                moveDir = Vector3.ProjectOnPlane(moveDir, hit.normal).normalized;
                if (slope >= maxClimbAngle)
                    rb.AddForce(-Physics.gravity, ForceMode.Acceleration);
            }
        }

        // Aquí aplicamos el movimiento físico
        Vector3 displacement = moveDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + displacement);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetTrigger(animator.GetFloat("Speed") > 0.1f ? "JumpMove" : "JumpIdle");
            isGrounded = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleFallingAnim()
    {
        bool falling = !isGrounded && rb.velocity.y < -0.1f;
        animator.SetBool("IsFalling", falling);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Car"))
        {
            isGrounded = true;
            animator.SetBool("IsGrounded", true);
        }
    }
}
