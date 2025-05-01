using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AudioSource))]
public class DogFollower : MonoBehaviour
{
    [Header("Seguimiento")]
    public Transform player;
    public float followDistance = 2f;
    public float runThreshold = 4f;

    [Header("Sonidos")]
    public AudioClip barkClip;           // ladrido cuando está lejos
    public float barkMinInterval = 5f;   // intervalo mínimo entre ladridos
    public float barkMaxInterval = 12f;  // intervalo máximo entre ladridos

    public AudioClip happyClip;          // sonido feliz cuando está al lado en idle
    public float happyMinInterval = 15f; // intervalo mínimo entre sonidos felices
    public float happyMaxInterval = 30f; // intervalo máximo entre sonidos felices

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;

    private float barkTimer;
    private float happyTimer;

    void Awake()
    {
        agent       = GetComponent<NavMeshAgent>();
        animator    = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // inicializamos los timers con un valor aleatorio para que no suene todo al inicio
        barkTimer  = Random.Range(barkMinInterval, barkMaxInterval);
        happyTimer = Random.Range(happyMinInterval, happyMaxInterval);
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // Calcula la posición objetivo desplazada a la derecha
        Vector3 rightOffset = player.right * 1.5f;   // 2 unidades a la derecha
        Vector3 forwardOffset = player.forward * 0.9f; // (opcional) 1 unidad hacia adelante
        Vector3 targetPos = player.position + rightOffset + forwardOffset;

        // 1) Movimiento
        if (dist > followDistance)
            agent.SetDestination(targetPos);
        else
            agent.ResetPath();

        // 2) Animaciones (con Speed como antes)
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        // 3) Sonidos
        HandleBark(dist);
        HandleHappy(dist, speed);
    }

    private void HandleBark(float dist)
    {
        // Si está suficientemente lejos, cuenta el tiempo
        if (dist > runThreshold)
        {
            barkTimer -= Time.deltaTime;
            if (barkTimer <= 0f)
            {
                audioSource.PlayOneShot(barkClip);
                // reinicia el timer con un nuevo intervalo aleatorio
                barkTimer = Random.Range(barkMinInterval, barkMaxInterval);
            }
        }
        else
        {
            // si se acerca, resetea el timer para que no barkee de golpe al alejarse de nuevo
            barkTimer = Random.Range(barkMinInterval, barkMaxInterval);
        }
    }

    private void HandleHappy(float dist, float speed)
    {
        // Solo cuando está cerca (idle) y prácticamente sin velocidad
        if (dist <= followDistance && speed < 0.1f)
        {
            happyTimer -= Time.deltaTime;
            if (happyTimer <= 0f)
            {
                audioSource.PlayOneShot(happyClip);
                happyTimer = Random.Range(happyMinInterval, happyMaxInterval);
            }
        }
        else
        {
            happyTimer = Random.Range(happyMinInterval, happyMaxInterval);
        }
    }
}
