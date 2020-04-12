using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform Character;
    [SerializeField] private float smoothingSpeed = 12.5f;
    private Vector3 offset = new Vector3 (0f,0f,-10f);

    private void Start()
    {
        transform.position = Character.position + offset;
    }
    void Update()
    {
        Vector3 desiredPosition = Character.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position,desiredPosition,smoothingSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
