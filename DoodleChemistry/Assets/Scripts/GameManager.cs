using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool won = false;
    [Header("Node Data")]
    [SerializeField] private GameObject connectorPrefab;
    [SerializeField] private Sprite[] connectionSprites;
    [SerializeField] private List<TargetLocation> nodes;

    [Header("Popup Hooks")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject moleculeCamPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // get all nodes in scene
        nodes.Clear();
        var nodeObjs = GameObject.FindGameObjectsWithTag("Node");
        foreach (var n in nodeObjs)
        {
            TargetLocation node;
            if (n.TryGetComponent<TargetLocation>(out node))
                nodes.Add(node);
        }

        // draw connections
        DrawConnectors(connectorPrefab, connectionSprites);
        winScreen.SetActive(false);
    }

    void DrawConnectors(GameObject prefab, Sprite[] sprites)
    {
        List<Connector> drawnConnectors = new List<Connector>();
        foreach (var node in nodes)
        {
            for (int i = 0; i < node.connectionStyle.Count; i++)
            {
                if (node.connections[i] == null) continue;
                var c = node.connectionStyle[i];

                if (drawnConnectors.Contains(c)) continue;
                drawnConnectors.Add(c);
                if (c.style >= sprites.Length)
                {
                    Debug.LogError("A node connection could not be built because it received an out-of-bounds sprite index.");
                    continue;
                }

                // connector hasn't been drawn yet, instantiate object and position it
                Vector2 position = Vector2.Lerp(node.transform.position, node.connections[i].transform.position, 0.5f);
                Vector2 dist = (Vector2)node.connections[i].transform.position - (Vector2)node.transform.position;
                Quaternion rot = Quaternion.FromToRotation(Vector3.right, dist);
                GameObject obj = GameObject.Instantiate(prefab, position, rot);

                float connectionSize = dist.magnitude - 0.5f;
                obj.transform.localScale = new Vector3(connectionSize, 1, 1);

                // set appearance
                var sp = obj.GetComponent<SpriteRenderer>();
                if (sp == null) continue;
                sp.sprite = sprites[c.style];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFilled())
        {
            // show feedback on whether the solution is right or wrong
            if (IsCorrect())
            {
                if(!won)
                WinLevel();
            }
        }

    }

    private void WinLevel()
    {
        won = true;
        SpawnMoleculeCam();
        StartCoroutine(WinPopupWithDelay(0.5f));
    }

    IEnumerator WinPopupWithDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        winScreen.SetActive(true);
    }

    private void SpawnMoleculeCam()
    {
        Camera rCam = GameObject.Instantiate(moleculeCamPrefab).GetComponent<Camera>();
        Vector3 targetPos = Vector3.zero;
        Vector2 minPos = Vector2.positiveInfinity;
        Vector2 maxPos = Vector2.negativeInfinity;
        foreach (var node in nodes)
        {
            Vector2 position = node.transform.position;
            targetPos += (Vector3)position;
            if (minPos.x > (position.x - 1.0f)) minPos.x = position.x - 1.0f;
            if (minPos.y > (position.y - 1.0f)) minPos.y = position.y - 1.0f;

            if (maxPos.x < (position.x + 1.0f)) maxPos.x = position.x + 1.0f;
            if (maxPos.y < (position.y + 1.0f)) maxPos.y = position.y + 1.0f;
        }
        targetPos = Vector2.Lerp(maxPos, minPos, 0.5f);
        targetPos.z = -10f;

        rCam.orthographicSize = Mathf.Max((maxPos.x - minPos.x),(maxPos.y - minPos.y)) - .25f;
        rCam.transform.position = targetPos;
    }

    bool IsFilled()
    {
        foreach (var node in nodes)
        {
            if (node == null || !node.HasElement()) return false;
        }

        return true;
    }

    bool IsCorrect()
    {
        foreach (var node in nodes)
        {
            if (!node.IsCorrect()) return false;
        }

        return true;
    }
}
