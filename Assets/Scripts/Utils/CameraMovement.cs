using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 50f;
    [SerializeField] private int _edgeThreshold = 50;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;

    private void FixedUpdate()
    {
        Vector3 movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, 0) * _speed* Time.fixedDeltaTime;
        Vector3 targetPosition = transform.position + movementVector;

        targetPosition.x = Mathf.Clamp(targetPosition.x, _minX, _maxX);

        transform.position = targetPosition;
    }
}
