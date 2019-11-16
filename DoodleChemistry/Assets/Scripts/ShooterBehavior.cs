using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehavior : MonoBehaviour
{
    private float rot; //Current rotation of the object
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
    }

    void HandleRotation()
    {
        float curAxis = Input.GetAxis("Horizontal") * rotationSpeed; //Gets current inpout for left and right
        rot += curAxis;

        rot = Mathf.Clamp(rot, -90, 90);
        this.transform.rotation = Quaternion.AngleAxis(-rot, Vector3.forward);
    }

    void Shoot()
    {

    }
}
