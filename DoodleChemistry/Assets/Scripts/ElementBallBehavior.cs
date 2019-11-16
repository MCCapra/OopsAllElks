using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBallBehavior : MonoBehaviour
{
    public float magnitude;
    public Vector3 impulse; //This is going to be the up vector of the launcher
    public Element element;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody2D>().AddForce((impulse * magnitude), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
