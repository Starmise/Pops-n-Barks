using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision2D : MonoBehaviour
{
    public Transform target; // El objeto que la c�mara sigue (e.g., el jugador)
    public BoxCollider2D cameraBounds; // El Collider que define los l�mites
    private Camera cam;
    private Vector3 minBounds;
    private Vector3 maxBounds;
    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        cam = Camera.main;

        // Calcula los l�mites del Collider
        minBounds = cameraBounds.bounds.min;
        maxBounds = cameraBounds.bounds.max;

        // Calcula el tama�o de la c�mara en unidades del mundo
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    void LateUpdate()
    {
        // Sigue al jugador
        Vector3 desiredPosition = target.position;

        // Restringe la posici�n dentro de los l�mites
        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
