using UnityEngine;
using System.Collections;

public class SlowTime : MonoBehaviour {

    [SerializeField] private float slowMo = 0.75f;
    [SerializeField] private float maxTime = 5.0f;
    public  bool canSlow;
    public float meterLevel;
    private bool maxSet = false;


    // Use this for initialization
    void Awake()
    {
        meterLevel = maxTime;
        canSlow = true;   
    }

    void FixedUpdate()
    { 
        if (maxSet)
            CanvasManager.instance.SetSliderMax(maxTime);

        if (Input.GetKey("left shift") && meterLevel > 0 && canSlow)
        {
            Time.timeScale = slowMo;
            meterLevel -= Time.fixedDeltaTime / slowMo;
        }
        else
        {            
            Time.timeScale = 1;
            meterLevel += Time.fixedDeltaTime;
            if (meterLevel > maxTime)
                canSlow = true;
        }
        if(meterLevel <= 0)
        {            
            canSlow = false;
        }
        meterLevel = Mathf.Clamp(meterLevel, 0, maxTime);

        //Update slider here
        CanvasManager.instance.SetSliderValue(meterLevel);

        
    }
    public void ResetMeter()
    {
        meterLevel = maxTime;
        //Update slider here
        CanvasManager.instance.SetSliderValue(meterLevel);
        
    }
}
