using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -7);
    public float smoothSpeed = 0.125f;
    public float rotationSpeed = 100f;

    private float currentRotationX = 0f;
    private float currentRotationY = 0f;

    void LateUpdate()
    {
        // Rotar la cámara con el ratón
        currentRotationX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        currentRotationY -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        currentRotationY = Mathf.Clamp(currentRotationY, -35, 60); // Evita que se voltee loca

        Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
