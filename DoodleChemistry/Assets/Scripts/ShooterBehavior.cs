using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Elements Enum
public enum Element { Sodium, Chlorine, Carbon, Oxygen, Hydrogen, Iron};
public class ShooterBehavior : MonoBehaviour
{
    private float rot; //Current rotation of the object
    public float rotationSpeed;
    public bool isFirable;

    [SerializeField]
    private GameObject elementBall;

    [SerializeField]
    private Element curElement;

    [SerializeField]
    private float magnitude;
    [SerializeField]
    private GameObject text;

    [SerializeField]
    private GameObject curBall;

    [SerializeField]
    private GameObject preview;

    [SerializeField]
    private Element[] launchElements; //Elements being fired by launcher

    private string[] symbols = { "Na", "Cl", "C", "O", "H", "Fe" };

    private int elementIndex; //Index of current element being fired

    public List<Sprite> elements;
    // Start is called before the first frame update
    void Start()
    {
        rot = 0;
        isFirable = true;
        elementIndex = 0;
        curElement = launchElements[elementIndex];
        text.GetComponent<TextMesh>().text = symbols[(int)curElement];
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
            elementIndex++;
            if(elementIndex >= launchElements.Length)
            {
                elementIndex = 0;
            }
            curElement = launchElements[elementIndex];
            text.GetComponent<TextMesh>().text = symbols[(int)curElement];
        }
    }

    void ChangeSprite()
    {
        preview.GetComponent<SpriteRenderer>().sprite = elements[(int)curElement];
    }
}
