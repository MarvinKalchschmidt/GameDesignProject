using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 50f;  
    [SerializeField] private float edgeThreshold;  

    private void Start(){
        edgeThreshold = Screen.width * 0.2f;
    }

    private void FixedUpdate()
    {       
        Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), 0f);
        /*if (Input.mousePosition.x < edgeThreshold)
        {
            inputVector.x -= 1f;
        }
        else if (Input.mousePosition.x > Screen.width - edgeThreshold)
        {
            inputVector.x += 1f;
        }
        else {
            inputVector.x = Input.GetAxis("Horizontal");
        }*/

        transform.Translate(inputVector * speed * Time.deltaTime);                  
    }
}
