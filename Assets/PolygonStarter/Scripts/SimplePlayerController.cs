using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float rotationSpeed = 10f;
    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;

    public Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator =GetComponent<Animator>();
    }

    public void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        // Movimiento WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Calculamos el ángulo basado en la cámara
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // Suavizamos el giro
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSpeed * Time.deltaTime);

            // Rotamos al personaje
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Movemos en dirección de la cámara
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;

            // Actualizar animador (si tienes parámetros como Speed)
            animator.SetFloat("Speed", direction.magnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }

    }

    private float turnSmoothVelocity;

    private void Jump()
    {
        // Saltar
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Detecta si toca el suelo para permitir saltar de nuevo
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
