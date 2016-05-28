using UnityEngine;
using System.Collections;

public class ControlSpeedLines : MonoBehaviour {

    public AnimationCurve curve;
    public Material TransitionMaterial;
    public float lowerBound = 0;
    public float upperBound;
    
    Rigidbody rb;

	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody>();        
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float range = upperBound - lowerBound;
        float val = (rb.velocity.magnitude-lowerBound)/range;
        val = Mathf.Clamp(val, 0, 1);
        TransitionMaterial.SetFloat("_Cutoff", curve.Evaluate(val));

    }
}
