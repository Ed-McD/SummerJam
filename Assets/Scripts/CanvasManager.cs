using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasManager : MonoBehaviour
{
    static CanvasManager _instance;
    public static CanvasManager instance { get { return _instance; } }

    [SerializeField] Text DataCollected;
    [SerializeField] Text BlocksDropped;
    [SerializeField] Slider slowmoSlider;

	// Use this for initialization
	void Start ()
    {
        _instance = this;
        SetScore(0);
        SetDroppedBlocks(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetScore(int score)
    {
        DataCollected.text = "Data Collected: " + score;
    }
    
    public void SetDroppedBlocks(int droppedBlocks)
    {
        BlocksDropped.text = "Blocks Dropped: " + droppedBlocks;
    }

    public void SetSliderValue(float slowValue)
    {
        slowmoSlider.value = slowValue;
    }
    public void SetSliderMax(float maxValue)
    {
        slowmoSlider.maxValue = maxValue;
    }
}
