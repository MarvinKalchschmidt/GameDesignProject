using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastCheck : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the camera to the mouse position in 2D
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // Check if the ray hits an object
            if (hit.collider != null)
            {
                // Get the GameObject that was hit
                GameObject hitObject = hit.collider.gameObject;

                // You can now access and manipulate the 'hitObject' as needed
                Debug.Log("Hit object: " + hitObject.name);

            }
        }
    }
}
