using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;    
    private GameObject cameraObject;    
    [SerializeField] private float lowerViewRange = 60;
    [SerializeField] private float upperViewRange = 60;
    public float sensitivity;
    public bool invertY;
    [SerializeField] private float moveSpeed = 500;
    [SerializeField] private float onGroundDrag;
    private int inversion;
    private bool grounded = false;
    Quaternion initialCamRot;
    Quaternion initialPlayerRot;

    // Use this for initialization
    void Awake()
    {
        cameraObject = GetComponentInChildren<Camera>().gameObject;
        rb = GetComponent<Rigidbody>();
        initialCamRot = cameraObject.transform.rotation;
        initialPlayerRot = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotation();
        Movement();

        if (grounded)
        {
            rb.velocity -= (rb.velocity * onGroundDrag) * Time.fixedDeltaTime;
        }
            

        SetGrounded(false);
    }

    void Movement()
    {
        float hz = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        float vt = Input.GetAxis("Vertical") * moveSpeed * Time.fixedDeltaTime;
        rb.AddRelativeForce(new Vector3(hz, 0, vt));

    }

    void Rotation()
    {
        float hz = Input.GetAxis("Mouse X") * sensitivity*100 * Time.fixedDeltaTime;
        float vt = Input.GetAxis("Mouse Y") * sensitivity*100 * Time.fixedDeltaTime;
        inversion = invertY ? 1 : -1;

        cameraObject.transform.Rotate(vt * inversion, 0, 0);
        transform.Rotate(0, hz, 0);
        if (cameraObject.transform.localEulerAngles.x > lowerViewRange && cameraObject.transform.localEulerAngles.x < 180)
        {
            cameraObject.transform.localEulerAngles = new Vector3(lowerViewRange, 0, 0);
        }
        if (cameraObject.transform.localEulerAngles.x < 360 - upperViewRange && cameraObject.transform.localEulerAngles.x > 180)
        {
            cameraObject.transform.localEulerAngles = new Vector3(360 - upperViewRange, 0, 0);
        }
    }

    public void SetGrounded(bool _grounded)
    {
        grounded = _grounded;
    }

    public void Reset()
    {
        rb.velocity = Vector3.zero;
        cameraObject.transform.rotation = initialCamRot;
        transform.rotation = initialPlayerRot;
    }

}
