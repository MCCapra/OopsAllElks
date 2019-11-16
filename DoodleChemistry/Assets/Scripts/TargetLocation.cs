using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class TargetLocation : MonoBehaviour
{
    [Header("Node Info")]
    [SerializeField] public List<TargetLocation> connections = new List<TargetLocation>();
    private List<int> styles = new List<int>();
    public List<Connector> connectionStyle = new List<Connector>();
    [SerializeField] private Element correctElement = Element.Carbon;

    private ElementBallBehavior ballElement = null;
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


        // play sound effects if ingame
        if(MusicManager.instance)
            MusicManager.instance.PlaySoundEffect(0);

        // make the ball a child of this and freeze its rigidbody
        capturedElement = obj;
        capturedElement.parent = transform;
        rb.simulated = false;
        ballElement = capturedElement.GetComponent<ElementBallBehavior>();

        // initialize values for capturing ball to move into position
        originalCapturePos = capturedElement.localPosition;
        shouldCapture = true;
        timeSinceCapture = 0.0f;
    }

    private void ClearBall()
    {
        GameObject.Destroy(capturedElement.gameObject);
        capturedElement = null;
        ballElement = null;
    }

    public void AddLink(TargetLocation node, int style = 0)
    {
        if (connections.Contains(node) || node.connections.Contains(this)) return;

        connections.Add(node);
        node.connections.Add(this);
        styles.Add(style);
        node.styles.Add(style);

        var c = new Connector(style);
        connectionStyle.Add(c);
        node.connectionStyle.Add(c);
    }

    private void GenerateConnectors()
    {
        foreach (var node in connections)
        {
            var c = new Connector(0);
            connectionStyle.Add(c);
            node.connectionStyle.Add(c);
        }
    }

    public bool HasElement()
    {
        return (ballElement != null);
    }

    public bool IsCorrect()
    {
        return (ballElement != null && ballElement.element == correctElement);
    }

    [ExecuteInEditMode]
    private void Awake()
    {
        return;
    }


    [ExecuteInEditMode]
    private void OnDestroy()
    {
        if(connections.Count > 0)
        {
            foreach (var node in connections)
            {
                node.connections.Remove(this);
            }
        }
    }

    public Element CorrectElement { get { return correctElement; } }

}

[System.Serializable]
public class Connector
{
    public Connector(int s=0)
    {
        style = s;
    }
    public int style = 0;
}