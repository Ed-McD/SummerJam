using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;    
    private GameObject cameraObject;    
    [SerializeField] private float viewRange = 60;
    public float sensitivity;
    public bool invertY;
    [SerializeField] private float moveSpeed = 500;
    [SerializeField] private float onGroundDrag;
    private int inversion;
    private bool grounded = false;

    // Use this for initialization
    void Awake()
    {
        cameraObject = GetComponentInChildren<Camera>().gameObject;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotation();
        Movement();

        if (grounded)
            rb.velocity -= (rb.velocity * onGroundDrag) * Time.fixedDeltaTime;

        SetGrounded(false);
    }

    void Movement()
    {
        float hz = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        float vt = Input.GetAxis("Vertical") * moveSpeed * Time.fixedDeltaTime;

        Debug.Log(hz +""+ vt);
        rb.AddRelativeForce(new Vector3(hz, 0, vt));

    }

    void Rotation()
    {
        float hz = Input.GetAxis("Mouse X") * sensitivity*100 * Time.fixedDeltaTime;
        float vt = Input.GetAxis("Mouse Y") * sensitivity*100 * Time.fixedDeltaTime;
        inversion = invertY ? 1 : -1;

        cameraObject.transform.Rotate(vt * inversion, 0, 0);
        transform.Rotate(0, hz, 0);
        if (cameraObject.transform.localEulerAngles.x > viewRange && cameraObject.transform.localEulerAngles.x < 180)
        {
            cameraObject.transform.localEulerAngles = new Vector3(viewRange, 0, 0);
        }
        if (cameraObject.transform.localEulerAngles.x < 360 - viewRange && cameraObject.transform.localEulerAngles.x > 180)
        {
            cameraObject.transform.localEulerAngles = new Vector3(360 - viewRange, 0, 0);
        }
    }

    public void SetGrounded(bool _grounded)
    {
        grounded = _grounded;
    }

}
