using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerGrow : MonoBehaviour
{
    public Vector3 bigSize = new Vector3(2f, 2f, 2f); // tamaño grande
    public Vector3 normalSize = new Vector3(1f, 1f, 1f); // tamaño normal
    public float growDuration = 20f; // cuánto dura agrandado

    private bool isBig = false;
    private Coroutine growCoroutine;

    public AudioClip powerUpSound;
    public AudioClip shrinkSound;
    public AudioSource audioSource;

    public CinemachineFreeLook freeLookCamera;
    private float topRadius, middleRadius, bottomRadius;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (freeLookCamera != null)
        {
            topRadius = freeLookCamera.m_Orbits[0].m_Radius;
            middleRadius = freeLookCamera.m_Orbits[1].m_Radius;
            bottomRadius = freeLookCamera.m_Orbits[2].m_Radius;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mushroom"))
        {
            if (growCoroutine != null)
            {
                StopCoroutine(growCoroutine);
            }
            growCoroutine = StartCoroutine(GrowTemporarily());

            // Reproducir sonido de power-up
            if (powerUpSound != null)
            {
                audioSource.PlayOneShot(powerUpSound);
            }

            Destroy(other.gameObject); // eliminar el hongo después de tocarlo
        }
    }

    private IEnumerator GrowTemporarily()
    {
        transform.localScale = bigSize;
        isBig = true;

        if (freeLookCamera != null)
        {
            freeLookCamera.m_Orbits[0].m_Radius = topRadius * 2f;
            freeLookCamera.m_Orbits[1].m_Radius = middleRadius * 2f;
            freeLookCamera.m_Orbits[2].m_Radius = bottomRadius * 2f;
        }

        float preShrinkTime = 1.5f;
        yield return new WaitForSeconds(growDuration - preShrinkTime);

        // Sonido de volver a tamaño normal
        if (shrinkSound != null)
        {
            audioSource.PlayOneShot(shrinkSound);
        }

        yield return new WaitForSeconds(preShrinkTime);

        transform.localScale = normalSize;
        isBig = false;

        if (freeLookCamera != null)
        {
            freeLookCamera.m_Orbits[0].m_Radius = topRadius;
            freeLookCamera.m_Orbits[1].m_Radius = middleRadius;
            freeLookCamera.m_Orbits[2].m_Radius = bottomRadius;
        }
    }
}
