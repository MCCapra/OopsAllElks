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
    [SerializeField]
    private GameObject curBall;
    [SerializeField]
    private GameObject preview;

    public List<Sprite> elements;
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
        ChangeSprite();
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
            
            curBall =  Instantiate(elementBall, new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z), transform.rotation);
            curBall.GetComponent<ElementBallBehavior>().impulse = this.transform.up;
            curBall.GetComponent<ElementBallBehavior>().element = curElement;
            curBall.GetComponent<ElementBallBehavior>().magnitude = magnitude;
            curBall.GetComponent<SpriteRenderer>().sprite = elements[(int)curElement];
            isFirable = false;
        }
    }

    void ChangeSprite()
    {
        preview.GetComponent<SpriteRenderer>().sprite = elements[(int)curElement];
    }
}
