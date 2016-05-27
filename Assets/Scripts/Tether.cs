using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class Tether : MonoBehaviour {

    private LineRenderer lr;
    private Rigidbody rb;
    private bool amTethered;
    private bool amPulling;
    public float pullSpeed = 10;
    public bool pullIgnoresGrav = false;
    private Vector3 tetherPoint;
    private float tetherLength;
    private Camera cam;
    private Material swingMat;
    private Material pullMat;

    [SerializeField]
    private Transform leftOffset;
    [SerializeField]
    private Transform rightOffset;

    // Use this for initialization
    void Awake()
    {
        if (!leftOffset)
        {
            leftOffset = transform;
        }

        if (!rightOffset)
        {
            rightOffset = transform;
        }

        if (!GetComponent<LineRenderer>())        
            gameObject.AddComponent<LineRenderer>();
        lr = GetComponent<LineRenderer>();
        lr.SetVertexCount(2);

        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        swingMat = lr.materials[0];
        pullMat = lr.materials[1];
        
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            amPulling = false;
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                setTetherPosition(hit.point);
                if(hit.collider.gameObject.tag == "block")
                {
                    hit.collider.gameObject.GetComponent<BlockBehaviour>().HitBlock();
                }
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            amTethered = false;
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                setPullPosition(hit.point);
            }
        }
        if (Input.GetButtonDown("Fire3"))
        {
            amTethered = false;
            amPulling = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (amTethered)
        {
            Vector3 vel = rb.velocity;
            Vector3 testPos = transform.position;
            lr.enabled = true;
            lr.SetPosition(0, leftOffset.position);
            lr.SetPosition(1, tetherPoint);

            if (lr.material != swingMat)
            {
                lr.material = swingMat;
            }

            vel += Physics.gravity * Time.fixedDeltaTime;
            vel -= (rb.drag * vel) * Time.fixedDeltaTime;
            testPos += vel * Time.fixedDeltaTime;


            if ((testPos - tetherPoint).magnitude > tetherLength)
            {

                testPos = tetherPoint + (testPos - tetherPoint).normalized * tetherLength;
                vel = (testPos - transform.position) / (Time.fixedDeltaTime);
                rb.velocity = vel;
            }

        }
        else if (amPulling)
        {
            Vector3 testPos = transform.position;
            Vector3 vel = rb.velocity;
            lr.enabled = true;
            lr.SetPosition(0, rightOffset.position);
            lr.SetPosition(1, tetherPoint);

            if (lr.material != pullMat)
            {
                lr.material = pullMat;                
            }

            //if (!pullIgnoresGrav)
            //    vel += Physics.gravity * Time.fixedDeltaTime;
            vel = pullIgnoresGrav ? vel - Physics.gravity * Time.fixedDeltaTime : vel + Physics.gravity * Time.fixedDeltaTime;

            vel -= (rb.drag * vel) * Time.fixedDeltaTime;
            testPos += vel * Time.fixedDeltaTime;
            testPos -= (testPos - tetherPoint).normalized * pullSpeed *  Time.fixedDeltaTime;
            vel = (testPos - transform.position) / Time.fixedDeltaTime;
            rb.velocity = vel;
        }
        else
        {
            lr.enabled = false;
        }
    }

    public void setPullPosition(Vector3 _hitLocation)
    {
        amPulling = true;
        tetherPoint = _hitLocation;
    }

    public void setTetherPosition(Vector3 _hitLocation)
    {
        amTethered = true;
        tetherPoint = _hitLocation;
        tetherLength = Vector3.Distance(tetherPoint, transform.position);
    }
}
