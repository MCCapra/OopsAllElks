using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBallBehavior : MonoBehaviour
{
    public float magnitude;
    public Vector3 impulse; //This is going to be the up vector of the launcher
    public Element element;
    private GameObject shooter;
    [SerializeField] private GameObject text;
    private string[] symbols = { "Na", "Cl", "C", "O", "H", "Fe" };
    private int numBounces;
    // Start is called before the first frame update
    void Start()
    {
        numBounces = 0;
        shooter = GameObject.Find("Shooter");
        this.GetComponent<Rigidbody2D>().AddForce((impulse * magnitude), ForceMode2D.Impulse);
        text.GetComponent<TextMesh>().text = symbols[(int)element];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            numBounces++;
            if(numBounces >= 4)
            {
                shooter.GetComponent<ShooterBehavior>().isFirable = true;
                Destroy(this.gameObject);
            }
        }
        else if(collision.transform.CompareTag("Death"))
        {
            shooter.GetComponent<ShooterBehavior>().isFirable = true;
            Destroy(this.gameObject);
        }
    }
}
