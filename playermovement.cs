using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class playermovement : MonoBehaviour
{

    Transform camra;
     Rigidbody rb;

    public float camrasecsitivity = 5f;
    public float mixy = -90f;
    public float maxy = 90f;
    public float rotationspeed = 5f;

    public float walkspeed = 10f;
    public float runspeed = 20f;
    public float maxspeed = 30f;
    public float jumpforce = 100f;

    public float extragravity = 45;

    float bodyrotationx;
    float camrotationy;
    Vector3 directionintentx;
    Vector3 directionintexty;

    float speed;

    public bool grounded;

    public LayerMask ground;
    public Transform groudncheck;

    public bool inair = false;
    void Start()
    {
        camra = Camera.main.transform;
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // adding some extra gravity
        //rb.AddForce(Vector3.down * extragravity);
        groud();
        lookrotation();

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump();
        }
    }

    private void FixedUpdate()
    {
        movement();
    }

    void lookrotation()
    {

        bodyrotationx += Input.GetAxis("Mouse X") * camrasecsitivity;
        camrotationy += Input.GetAxis("Mouse Y") * camrasecsitivity;

        camrotationy = Mathf.Clamp(camrotationy, mixy, maxy);

        Quaternion camtragatrotation = Quaternion.Euler(-camrotationy, 0, 0);
        Quaternion bodyrotation = Quaternion.Euler(0, bodyrotationx, 0);

        // handel rotation 
        transform.rotation = Quaternion.Lerp(transform.rotation, bodyrotation, Time.deltaTime * rotationspeed);

        camra.localRotation = Quaternion.Lerp(camra.localRotation, camtragatrotation, Time.deltaTime * rotationspeed);
    }

    void movement()
    {
        directionintentx = camra.right;
        directionintentx.y = 0;
        directionintentx.Normalize();

        directionintexty = camra.forward;
        directionintexty.y = 0;
        directionintexty.Normalize();

        rb.velocity = directionintexty * Input.GetAxis("Vertical") * speed + directionintentx * Input.GetAxis("Horizontal") * speed + Vector3.up * rb.velocity.y;

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxspeed);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runspeed;
        }
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            speed = walkspeed;
        }
    }
    bool groud() 
    {

        if (Physics.CheckSphere(groudncheck.position, 3.5f, ground))
        {
            grounded = true;
            inair = false;
        }

        else
        {
            grounded = false;
            inair = true;
        }

        return grounded;
        
    }

    void jump()
    {
        rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
    }
}    

