using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class Tether : MonoBehaviour {

    private LineRenderer lr;
    private Rigidbody rb;
    private bool amTethered;
    private Vector3 tetherPoint;
    private float tetherLength;
    private Camera cam;

    [SerializeField]
    private Transform offset;

    // Use this for initialization
    void Awake()
    {
        if (!offset)
        {
            offset = transform;
        }

        if (!GetComponent<LineRenderer>())        
            gameObject.AddComponent<LineRenderer>();
        lr = GetComponent<LineRenderer>();
        lr.SetVertexCount(2);

        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                setPosition(hit.point);
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            amTethered = false;
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
            lr.SetPosition(0, offset.position);
            lr.SetPosition(1, tetherPoint);

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
        else
        {
            lr.enabled = false;
        }
    }

    public void setPosition(Vector3 _hitLocation)
    {
        amTethered = true;
        tetherPoint = _hitLocation;
        tetherLength = Vector3.Distance(tetherPoint, transform.position);
    }
}
