using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

[EditorTool("Node Adder", typeof(TargetLocation))]
public class TargetNodeCreationTool : EditorTool
{
    //[Range(0.1f,1.0f)]
    public static float handleSize = 0.1f;

    private static bool creatingNewNode;
    private static TargetLocation selectedNode = null;
    private void OnEnable()
    {
        creatingNewNode = false;
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
                if (Handles.Button(node.transform.position, Quaternion.identity, handleSize, handleSize, Handles.SphereHandleCap))
                {
                    creatingNewNode = (selectedNode == node);
                    selectedNode = node;
                }

                // draw connections if they exist
                if (node.connections.Count > 0)
                {
                    foreach (var obj in node.connections)
                    {
                        if(obj != null)
                        Handles.DrawLine(node.transform.position, obj.transform.position);
                    }
                }
            }
        }

        // draw for new node loc
        if (creatingNewNode)
        {
            Vector3 fromPos = selectedNode.transform.position;
            Vector3 toPos = (Vector2)(Vector3.ProjectOnPlane(Camera.current.ScreenToWorldPoint(Input.mousePosition),Vector3.forward));

            Debug.Log(toPos.z);
            using (new Handles.DrawingScope(Color.yellow))
            {
                Handles.DrawDottedLine(fromPos, toPos,100);
            }

            // check mouse input state
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                            CreateNewNode(selectedNode.gameObject, (Vector2)toPos);

                            creatingNewNode = false;
                    }
                    break;

            }
        }
    }

    private void CreateNewNode(GameObject prefab, Vector2 location)
    {
        GameObject obj = GameObject.Instantiate(prefab, location, Quaternion.identity);
        TargetLocation newNode = obj.GetComponent<TargetLocation>();

        // clear connections for the new node to 0
        newNode.connections.Clear();

        // register connection
        newNode.connections.Add(selectedNode);
        selectedNode.connections.Add(newNode);
    }

} // end of class
