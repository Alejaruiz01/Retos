using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarResetOnEnter : MonoBehaviour
{
    [Tooltip("Referencia al punto de respawn")]
    public Transform spawnPoint;

    public AudioClip teleportSound;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Teleporta al jugador al punto de spawn
            other.transform.position = spawnPoint.position;
            other.transform.rotation = spawnPoint.rotation;

            // Opcional: restablece también velocidad
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = Vector3.zero;

            if (teleportSound != null)
            {
                audioSource.PlayOneShot(teleportSound);
            }
        }
    }
}

