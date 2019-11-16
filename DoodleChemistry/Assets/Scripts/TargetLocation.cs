using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TargetLocation : MonoBehaviour
{


    private Transform capturedElement = null;

    private bool shouldCapture = false; // whether the "animation" type stuff should occur
    private float timeSinceCapture = 0.0f;
    [SerializeField] private AnimationCurve capturePath;
    private void Update()
    {
        //if (capturedElement)
        //{
        //    if (shouldCapture)
        //    {
        //        // move the captured bit closer based on path
        //        timeSinceCapture += Time.deltaTime;
        //        float lerpAmt = capturePath.Evaluate(timeSinceCapture);
        //        Vector3 loc = capturedElement.localPosition;
        //        capturedElement.localPosition = Vector3.Lerp(loc, Vector3.zero, lerpAmt);

        //        // stop capturing if the object is close enough
        //        if (lerpAmt >= 1.0f)
        //        {
        //            capturedElement.localPosition = Vector3.zero;
        //            shouldCapture = false;
        //        }
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            // destroy existing ball if there is one
            if (capturedElement)
            {
                ClearBall();
            }
            CaptureBall(collision.transform);
        }
    }

    private void CaptureBall(Transform obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (!rb) return;

        capturedElement = obj;
        capturedElement.parent = transform;

        rb.simulated = false;

        capturedElement.localPosition = Vector3.zero;
        // initialize values for capturing ball to move into position
        //shouldCapture = true;
        //timeSinceCapture = 0.0f;
    }

    private void ClearBall()
    {
        GameObject.Destroy(capturedElement.gameObject);
        capturedElement = null;
    }

}
