using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Elements Enum
public enum Element { Sodium, Chlorine, Carbon, Oxygen, Hydrogen, Iron};
public class ShooterBehavior : MonoBehaviour
{
    private float rot; //Current rotation of the object
    public float rotationSpeed;
    [SerializeField]
    private GameObject elementBall;
    [SerializeField]
    private Element curElement;
    [SerializeField]
    private float magnitude;

    public bool isFirable;

    private GameObject curBall;
    // Start is called before the first frame update
    void Start()
    {
        rot = 0;
        isFirable = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        Shoot();
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
        if(isFirable && Input.GetKeyDown(KeyCode.Space))
        {
            
            curBall =  Instantiate(elementBall, this.transform.position, transform.rotation);
            curBall.GetComponent<ElementBallBehavior>().impulse = this.transform.up;
            curBall.GetComponent<ElementBallBehavior>().element = curElement;
            curBall.GetComponent<ElementBallBehavior>().magnitude = magnitude;

            isFirable = false;
        }
    }
}
