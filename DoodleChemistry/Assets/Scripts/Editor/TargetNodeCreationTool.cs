using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

[EditorTool("Node Adder", typeof(TargetLocation))]
public class TargetNodeCreationTool : EditorTool
{
    [Range(0.1f, 0.5f)]
    public float handleSize = 0.1f;
    public GameObject nodePrefab = null;

    private static bool creatingNewNode;
    private static TargetLocation selectedNode = null;
    private void OnEnable()
    {
        creatingNewNode = false;
        nodePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Pre-Fabs/Target.prefab");
    }

    public override void OnToolGUI(EditorWindow window)
    {
        var nodes = FindObjectsOfType<TargetLocation>();
        if (nodes == null) return;

        foreach (var node in nodes)
        {

            Color drawColor = (node == selectedNode) ? Color.green : Color.white;
            using (new Handles.DrawingScope(drawColor))
            {
                // draw button to select active node
                if (Handles.Button(node.transform.position, Quaternion.identity, handleSize, handleSize, Handles.CircleHandleCap))
                {
                    bool isSelected = (selectedNode == node);
                    if (!isSelected && creatingNewNode)
                    {
                        LinkNodes(selectedNode, node);
                    }

                    creatingNewNode = isSelected;
                    selectedNode = node;
                }

                // draw connections if they exist
                if (node.connections.Count > 0)
                {
                    foreach (var obj in node.connections)
                    {
                        if (obj != null)
                            Handles.DrawLine(node.transform.position, obj.transform.position);
                    }
                }
            }
        }

        // draw for new node loc
        if (creatingNewNode)
        {
            Vector3 fromPos = selectedNode.transform.position;
            Vector3 toPos = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(Event.current.mousePosition);
            toPos.y = -(toPos.y - (2 * SceneView.currentDrawingSceneView.camera.transform.position.y));

            using (new Handles.DrawingScope(Color.yellow))
            {
                Handles.DrawLine(fromPos, toPos);
            }

            // check mouse input state
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    switch (e.keyCode)
                    {
                        case KeyCode.Space:
                            {
                                if (nodePrefab != null)
                                    CreateNewNode(nodePrefab, (Vector2)toPos);
                                else
                                    Debug.LogError("No Node Prefab is set. Go to Window->General->Active Tool to set one.");

                                creatingNewNode = false;
                                break;
                            }
                        case KeyCode.Escape:
                            {
                                creatingNewNode = false;
                                break;
                            }
                        case KeyCode.Alpha1:
                            if (nodePrefab != null)
                                CreateNewNode(nodePrefab, (Vector2)toPos, 0);
                            else
                                Debug.LogError("No Node Prefab is set. Go to Window->General->Active Tool to set one.");

                            creatingNewNode = false;
                            break;
                        case KeyCode.Alpha3:
                            if (nodePrefab != null)
                                CreateNewNode(nodePrefab, (Vector2)toPos, 1);
                            else
                                Debug.LogError("No Node Prefab is set. Go to Window->General->Active Tool to set one.");

                            creatingNewNode = false;
                            break;
                        case KeyCode.Alpha4:
                            if (nodePrefab != null)
                                CreateNewNode(nodePrefab, (Vector2)toPos, 2);
                            else
                                Debug.LogError("No Node Prefab is set. Go to Window->General->Active Tool to set one.");

                            creatingNewNode = false;
                            break;
                    }
                    break;

            }
        }
        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.BackQuote)
                foreach (var node in nodes)
                {
                    node.gameObject.name = "Node_" + node.CorrectElement.ToString();
                }
        }
    }

    private void CreateNewNode(GameObject prefab, Vector2 location, int style = 0)
    {
        // create a new node identical to the first one
        GameObject obj = GameObject.Instantiate(prefab, location, Quaternion.identity);
        TargetLocation newNode = obj.GetComponent<TargetLocation>();
        obj.name = selectedNode.gameObject.name;

        // clear connections for the new node to 0
        newNode.connections.Clear();

        // register connection
        LinkNodes(newNode, selectedNode, style);
    }

    private void LinkNodes(TargetLocation n1, TargetLocation n2, int style = 0)
    {
        if (n1.connections.Contains(n2) || n2.connections.Contains(n1)) return;

        n1.AddLink(n2, style);
    }
} // end of class
