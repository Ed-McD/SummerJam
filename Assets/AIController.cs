using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {

    public List<Vector3> Waypoints = new List<Vector3>();
    int progression = 0;
    [SerializeField] private float leeway = 0;
    [SerializeField] private float speed =1;
    [SerializeField] private float rotSpeed = 10;


    [SerializeField]
    private float coolDownLength;
    private float cdTimer = 0.0f;
    private bool cdActive = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        CoolDown();
        Move();
    }
    void CoolDown()
    {
        if (cdActive)
        {
            cdTimer += Time.deltaTime;
            if (cdTimer>= coolDownLength)
            {
                cdActive = false;
                cdTimer = 0.0f;
            }
        }
    }
    void Move()
    {
        if (Waypoints.Count > 0)
        {
            if (Vector3.Distance(transform.position, Waypoints[progression]) <= leeway)
            {
                progression = progression >= (Waypoints.Count - 1) ? progression = 0 : progression+1;
            }
            if (transform.rotation == Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Waypoints[progression] - transform.position), rotSpeed * Time.fixedDeltaTime))
            {
                transform.position = Vector3.MoveTowards(transform.position, Waypoints[progression], speed * Time.fixedDeltaTime);
            }
            if (Waypoints[progression]-transform.position!= Vector3.zero)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Waypoints[progression]-transform.position), rotSpeed * Time.fixedDeltaTime);
        }
    }
}

