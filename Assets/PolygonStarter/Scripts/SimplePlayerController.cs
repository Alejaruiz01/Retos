using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSmoothTime = 0.1f;
    public float jumpForce = 5f;
    public float maxClimbAngle = 45f;
    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;

    private float turnSmoothVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        //Bloquear mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        Move();
        Jump();

        // Detectar caída
        bool falling = !isGrounded && rb.velocity.y < -0.1f;
        animator.SetBool("IsFalling", falling);
    }

    private void Move()
    {
        // Movimiento WASD
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            // 2) Obtener rotación real de la Main Camera
            Transform cam = Camera.main.transform;
            Vector3 camForward = cam.forward;
            Vector3 camRight   = cam.right;
            camForward.y = 0f;   // descartamos inclinación vertical
            camRight.y   = 0f;
            camForward.Normalize();
            camRight.Normalize();

            // 3) Dirección de movimiento RELATIVA a la cámara
            Vector3 moveDir = camForward * vertical + camRight * horizontal;
            moveDir.Normalize();

            // 4) Rotación suave hacia moveDir
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                                                ref turnSmoothVelocity,
                                                rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //  --- corregir para pendientes ---
            RaycastHit hit;
            float rayDistance = 1.1f; // ajusta según mitad de altura del collider
            if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance) && hit.collider.CompareTag("Ground"))
                {
                    float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                    // Proyectamos sobre la pendiente siempre
                    moveDir = Vector3.ProjectOnPlane(moveDir, hit.normal).normalized;

                    // Si la pendiente es muy empinada, cancelamos la gravedad
                    if (slopeAngle >= maxClimbAngle)
                    {
                        // Esto añade una aceleración hacia arriba igual a la gravedad
                        rb.AddForce(-Physics.gravity, ForceMode.Acceleration);
                    }
                }

            // 5) Mover personaje
            Vector3 displacement = moveDir * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + displacement);

            // 6) Animación
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            float currentSpeed = animator.GetFloat("Speed");
            isGrounded = false;
            animator.SetBool("IsGrounded", false);

            if (currentSpeed > 0.05f)
            {
                // Personaje está moviéndose → Salto en movimiento
                animator.SetTrigger("JumpMove"); // Asegúrate de tener este Trigger en el Animator

                // Añadimos impulso extra porque la animación no sube tanto
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else
            {
                // Personaje está quieto → Salto desde idle
                animator.SetTrigger("JumpIdle"); // Asegúrate de tener este Trigger en el Animator
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Detecta si toca el suelo para permitir saltar de nuevo
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Car"))
        {
            isGrounded = true;
            animator.SetBool("IsGrounded", true);
        }
    }
}
