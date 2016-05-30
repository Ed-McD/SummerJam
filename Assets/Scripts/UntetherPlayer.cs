using UnityEngine;
using System.Collections;

public class UntetherPlayer : MonoBehaviour {

    public float forceMod = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<Tether>().Reset();
            Vector3 dpos = col.transform.position - transform.position; 
            Vector3 normDpos = dpos.normalized;
            col.GetComponent<Rigidbody>().AddForce(dpos.sqrMagnitude * normDpos * forceMod);
        }
    }
}
