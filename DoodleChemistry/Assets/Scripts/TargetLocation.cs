using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class TargetLocation : MonoBehaviour
{
    [Header("Node Info")]
    [SerializeField] private List<TargetLocation> connections;


    private Transform capturedElement = null;
    [SerializeField] private GameObject shooter;

    private bool shouldCapture = false; // whether the "animation" type stuff should occur
    private float timeSinceCapture = 0.0f;
    private Vector3 originalCapturePos = Vector3.zero;
    [SerializeField] private AnimationCurve capturePath;

    private void Start()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        shooter = GameObject.Find("Shooter");
    }

    private void Update()
    {
        if (capturedElement)
        {
            if (shouldCapture)
            {
                // move the captured bit closer based on path
                timeSinceCapture += Time.deltaTime;
                float lerpAmt = capturePath.Evaluate(timeSinceCapture);
                capturedElement.localPosition = Vector3.LerpUnclamped(originalCapturePos, Vector3.zero, lerpAmt);

                // stop capturing if the object is close enough
                if (timeSinceCapture >= capturePath.keys[capturePath.length-1].time)
                {
                    originalCapturePos = Vector3.zero;
                    capturedElement.localPosition = Vector3.zero;
                    shouldCapture = false;
                }
            }
        }
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
            shooter.GetComponent<ShooterBehavior>().isFirable = true;
        }
    }

    private void CaptureBall(Transform obj)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (!rb) return;

        capturedElement = obj;
        capturedElement.parent = transform;

        rb.simulated = false;

        //capturedElement.localPosition = Vector3.zero;
        // initialize values for capturing ball to move into position
        originalCapturePos = capturedElement.localPosition;
        shouldCapture = true;
        timeSinceCapture = 0.0f;
    }

    private void ClearBall()
    {
        GameObject.Destroy(capturedElement.gameObject);
        capturedElement = null;
    }

}
