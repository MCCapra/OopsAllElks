using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float speed;
    [SerializeField]
    private float leftEndPoint;
    [SerializeField]
    private float rightEndPoint;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float ping = Mathf.PingPong((Time.time / speed), 1.0f);

        Vector3 newPos = Vector3.Lerp(new Vector3(leftEndPoint, this.transform.position.y, this.transform.position.z), new Vector3(rightEndPoint, this.transform.position.y, this.transform.position.z), ping);
        this.transform.position = newPos;

    }
}
