using UnityEngine;
using System.Collections;

public class DrawCrossHair : MonoBehaviour
{

   
    [SerializeField]
    private Texture2D crossHair;
    private Rect screenPos;
    [SerializeField]
    private float scaleFactor = 1;

    // Use this for initialization
    void Awake()
    {
        screenPos = new Rect((Screen.width- crossHair.width * scaleFactor) / 2, (Screen.height-crossHair.height*scaleFactor) / 2, crossHair.width*scaleFactor, crossHair.height*scaleFactor);
    }

    // Update is called once per frame
    void OnGUI()
    {
        GUI.DrawTexture(screenPos, crossHair);
    }
}
