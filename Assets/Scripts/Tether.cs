using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private TetheredObject tetheredObject;
    public List<TetherReg> activeTethers;
    public bool testingMultipleTethers = true;
    public GameObject lrObject;

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
        activeTethers = new List<TetherReg>();
        if (testingMultipleTethers)
        {
            activeTethers.Add(new TetherReg());
            activeTethers.Add(new TetherReg());
            foreach (TetherReg t in activeTethers)
            {
                t.amTethered = false;
                t.lrObject = Instantiate(lrObject);
                t.lrObject.transform.SetParent(transform);
                t.lrObject.transform.localPosition = Vector3.zero;
                t.lr = t.lrObject.GetComponent<LineRenderer>();
                t.lr.SetVertexCount(2);
                t.lr.enabled = false;
            }
        }
    }

    void Update()
    {
        if (testingMultipleTethers)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                amPulling = false;
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
                {
                    setActiveTether(hit,0);
                    if (hit.collider.gameObject.tag == "block")
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
                    setActiveTether(hit,1);
                    if (hit.collider.gameObject.tag == "block")
                    {
                        hit.collider.gameObject.GetComponent<BlockBehaviour>().HitBlock();
                    }
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                amPulling = false;
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
                {
                    setTetherPosition(hit);
                    if (hit.collider.gameObject.tag == "block")
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
                    setPullPosition(hit);
                    if (hit.collider.gameObject.tag == "block")
                    {
                        hit.collider.gameObject.GetComponent<BlockBehaviour>().HitBlock();
                    }
                }
            }
            if (Input.GetButtonDown("Fire3"))
            {
                amTethered = false;
                amPulling = false;
            }
        }
    }

    void LateUpdate()
    {
        if (amTethered)
        {
            lr.enabled = true;
            lr.SetPosition(0, leftOffset.position);
            lr.SetPosition(1, tetherPoint);

            if (lr.material != swingMat)
            {
                lr.material = swingMat;
            }
        }
        else if (amPulling)
        {
            lr.enabled = true;
            lr.SetPosition(0, rightOffset.position);
            lr.SetPosition(1, tetherPoint);

            if (lr.material != pullMat)
            {
                lr.material = pullMat;
            }
        }
        else
        {
            lr.enabled = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region multipleTethers
        if (testingMultipleTethers)
        {
            foreach (TetherReg t in activeTethers)
            {
                if (t.amTethered)
                {
                    t.tetherPoint = t.tetheredObject.attachedTo.gameObject.transform.position + t.tetheredObject.relativeHitPoint;
                    Vector3 vel = rb.velocity;
                    Vector3 testPos = transform.position;
                    t.lr.enabled = true;
                    t.lr.SetPosition(0, t.lrObject.transform.position);
                    t.lr.SetPosition(1, t.tetherPoint);

                    //if (lr.material != swingMat)
                    //{
                    //    lr.material = swingMat;
                    //}

                    vel += Physics.gravity * Time.fixedDeltaTime;
                    vel -= (rb.drag * vel) * Time.fixedDeltaTime;
                    testPos += vel * Time.fixedDeltaTime;

                    if ((testPos - t.tetherPoint).magnitude > t.tetherLength)
                    {
                        testPos = t.tetherPoint + (testPos - t.tetherPoint).normalized * t.tetherLength;
                        vel = (testPos - transform.position) / (Time.fixedDeltaTime);
                        rb.velocity = vel;
                    }
                }
                else
                {
                    t.lr.enabled = false;
                }
            }
        }
        #endregion

        else
        {
            if (tetheredObject.attachedTo == null)
            {
                amTethered = false;
                amPulling = false;
            }

            if (amTethered)
            {
                tetherPoint = tetheredObject.attachedTo.gameObject.transform.position + tetheredObject.relativeHitPoint;
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
                tetherPoint = tetheredObject.attachedTo.gameObject.transform.position + tetheredObject.relativeHitPoint;
                Vector3 testPos = transform.position;
                Vector3 vel = rb.velocity;
                lr.enabled = true;
                lr.SetPosition(0, rightOffset.position);
                lr.SetPosition(1, tetherPoint);

                if (lr.material != pullMat)
                {
                    lr.material = pullMat;
                }

                vel = pullIgnoresGrav ? vel - Physics.gravity * Time.fixedDeltaTime : vel + Physics.gravity * Time.fixedDeltaTime;

                vel -= (rb.drag * vel) * Time.fixedDeltaTime;
                testPos += vel * Time.fixedDeltaTime;
                testPos -= (testPos - tetherPoint).normalized * pullSpeed * Time.fixedDeltaTime;
                vel = (testPos - transform.position) / Time.fixedDeltaTime;
                rb.velocity = vel;
            }
            else
            {
                lr.enabled = false;
            }
        }

    }

    public void setPullPosition(RaycastHit _hit)
    {
        amPulling = true;
        tetheredObject = new TetheredObject() { attachedTo = _hit.collider.gameObject, relativeHitPoint = _hit.point - _hit.collider.gameObject.transform.position};

    }

    public void setTetherPosition(RaycastHit _hit)
    {
        amTethered = true;
        tetheredObject = new TetheredObject() { attachedTo = _hit.collider.gameObject, relativeHitPoint = _hit.point - _hit.collider.gameObject.transform.position};
        tetherLength = Vector3.Distance(tetheredObject.attachedTo.gameObject.transform.position + tetheredObject.relativeHitPoint, transform.position);
    }

    public void setActiveTether(RaycastHit _hit, int _index)
    {
        activeTethers[_index].amTethered = true;
        activeTethers[_index].tetheredObject = new TetheredObject() { attachedTo = _hit.collider.gameObject, relativeHitPoint = _hit.point - _hit.collider.gameObject.transform.position};
        activeTethers[_index].tetherLength = Vector3.Distance(activeTethers[_index].tetheredObject.attachedTo.gameObject.transform.position + activeTethers[_index].tetheredObject.relativeHitPoint, transform.position);
    }

    public void Reset()
    {
        amTethered = false;
        amPulling = false;
    }

}
public struct TetheredObject
{
    public GameObject attachedTo;
    public Vector3 relativeHitPoint;
}
public class TetherReg
{
    public bool amTethered;
    public Vector3 tetherPoint;
    public TetheredObject tetheredObject;
    public float tetherLength;
    public GameObject lrObject;
    public LineRenderer lr;

}
