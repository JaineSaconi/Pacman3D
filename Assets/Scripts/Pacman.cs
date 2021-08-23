using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    public float vel = 5;
    public float velRot = 100;
    private Quaternion rotIni;
    public LayerMask unwakbale;
    private Rigidbody rbd;
    private float rotY = 0;


    public GameObject end1;
    public GameObject end2;
    public Vector3 initEnd1;
    public Vector3 initEnd2;

    //reset
    Vector3 initPosition;

    void Start()
    {
        rbd = GetComponent<Rigidbody>();
        rotIni = transform.localRotation;
        initPosition = transform.position;
        initEnd1 = end1.transform.position;
        initEnd2 = end2.transform.position;
    }

    void Update()
    {
        float moveFrente = Input.GetAxis("Vertical");
        float moveLado = Input.GetAxis("Horizontal");

        rotY += Input.GetAxisRaw("Mouse X") * velRot * Time.deltaTime;

        Quaternion lado = Quaternion.AngleAxis(rotY, Vector3.up);

        transform.localRotation = rotIni * lado;

        rbd.velocity = transform.TransformDirection(new Vector3(moveLado * vel, rbd.velocity.y, moveFrente * vel));
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "end1")
        {
            transform.position = new Vector3(initEnd2.x - 3, transform.position.y, transform.position.z);
        }
        else if (col.gameObject.name == "end2")
        {
            transform.position = new Vector3(initEnd1.x + 3, transform.position.y, transform.position.z); ; 
        }
    }

    public void Reset()
    {
        transform.position = initPosition;
    }

}
