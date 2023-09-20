using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetectionUtil : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _detectionDelay = 0.1f;
    [SerializeField] private DetectionType _detectionType;
    //Event?
    [SerializeField] private Vector3 _detectionOffset;
    [Range(0.1f, 20f)]
    [SerializeField] private float _radius;
    [SerializeField] private Vector2 _detectionSize;
    [SerializeField] private List<GameObject> _detectedTargets;

    private bool _detectedTargetInLineOfSight;
    public List<GameObject> DetectedTargets { 
        get => _detectedTargets; 
        set { 
            _detectedTargets = value; 
            //DetectedTargetInLineOfSight = false; 
        }     
    }

    public bool DetectedTargetInLineOfSight { get => _detectedTargetInLineOfSight; set => _detectedTargetInLineOfSight = value; }

    public bool _showGizmos = true;
    public bool _checkForLineOfSight = false;
    private bool isDetecting = false;

    private void Start()
    {
        _detectedTargets = new List<GameObject>();
        StartCoroutine(TargetDetectionCouroutine());
    }

    private void Update()
    {
        if (_detectedTargets != null && _checkForLineOfSight)
        {
            /**Has to be tested and made adaptive, Not implemented, just a good idea**/
            DetectedTargetInLineOfSight = CheckTargetInLineOfSight(_detectedTargets[0]);
        }
    }

    /**Has to be tested again, LayerMasks could not be correct**/
    private bool CheckTargetInLineOfSight(GameObject target)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position - transform.position, _radius, _targetLayer);

        if(hit.collider != null)
        {
            return (_targetLayer & (1 << hit.collider.gameObject.layer)) != 0;
        }
        return false;
    }

    private IEnumerator TargetDetectionCouroutine()
    {       
        yield return new WaitForSeconds(_detectionDelay);
        PerformTargetDetection(_detectionType);
        StartCoroutine(TargetDetectionCouroutine());
    }

    /*
    private void PerformTargetDetection(DetectionType detectionType)
    {
        List<GameObject> newDetectedTargets = new List<GameObject>();

        Collider2D[] colliders = detectionType == DetectionType.Box2D ? 
            Physics2D.OverlapBoxAll(transform.position, _detectionSize, 0, _targetLayer) : 
            Physics2D.OverlapCircleAll(transform.position, _radius, _targetLayer);

        foreach (Collider2D collider in colliders)
        {
            GameObject foundTarget = collider.gameObject;

            if (!TargetAlreadyDetected(foundTarget))
            {
                Debug.Log("Enemy Found");
                DetectedTargets.Add(foundTarget);
                newDetectedTargets.Add(foundTarget); // Store newly detected targets in a separate list.
            }
        }

        foreach (GameObject target in DetectedTargets)
        {
            if (!newDetectedTargets.Contains(target))
            {
                Debug.Log("Enemy Lost");
                DetectedTargets.Remove(target);
            }
        }
    }*/

    private void PerformTargetDetection(DetectionType detectionType)
    {
        List<GameObject> detectedTargets = new List<GameObject>();

        Collider2D[] colliders = detectionType == DetectionType.Box2D ?
            Physics2D.OverlapBoxAll(transform.position + _detectionOffset, _detectionSize, 0, _targetLayer) :
            Physics2D.OverlapCircleAll(transform.position + _detectionOffset, _radius, _targetLayer);

        foreach (Collider2D collider in colliders)
        {
            GameObject newTarget = collider.gameObject;
            if (collider != null)
            {
                detectedTargets.Add(collider.gameObject);
            }
        }
        DetectedTargets = detectedTargets;
    }



    private void OnDrawGizmos()
    {

        if (_showGizmos)
        {
            Gizmos.color = Color.green;
            if(DetectedTargets.Count >= 1)
            {
                Gizmos.color = Color.red;
            }

            if (_detectionType == DetectionType.Box2D)
            {
                Gizmos.DrawWireCube(transform.position + _detectionOffset, _detectionSize);
            }
            else if(_detectionType == DetectionType.Circle2D)
            {
                Gizmos.DrawWireSphere(transform.position + _detectionOffset, _radius);
            }            
        }
    }
}

public enum DetectionType
{
    Box2D = 0,
    Circle2D = 1
}
