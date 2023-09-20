using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 50f;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private Animator _cameraMovementAnimator;

    public static event Action OnAnimationFinished;


    public float MinX { get => _minX; set => _minX = value; }
    public float MaxX { get => _maxX; set => _maxX = value; }

    private void Start()
    {
        if (_cameraMovementAnimator == null)
        {
            _cameraMovementAnimator = GetComponent<Animator>();
        }
    }

    private void FixedUpdate()
    {
        if(GameManager.Instance.ControllsEnabled)
        {
            Vector3 movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, 0) * _speed * Time.fixedDeltaTime;

            Vector3 targetPosition = transform.position + movementVector;

            MoveCamera(targetPosition);
        }        
    }

    private void MoveCamera(Vector3 targetPosition)
    {
        targetPosition.x = Mathf.Clamp(targetPosition.x, _minX, _maxX);

        transform.position = targetPosition;
    }

    public void AnimationFinished()
    {
        _cameraMovementAnimator.enabled = false;
        OnAnimationFinished?.Invoke();
    }
}
