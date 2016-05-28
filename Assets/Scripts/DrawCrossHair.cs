using UnityEngine;
using System.Collections;

public class DrawCrossHair : MonoBehaviour
{

   
    [SerializeField]
    private Texture2D crossHair;
    private Rect screenPos;
    [SerializeField]
    private float scaleFactor = 1;
    Vector2 lastWindowSize;

    // Use this for initialization
    void Awake()
    {
        CalculateRect();
    }
    void Update()
    {
        if (lastWindowSize.x != Screen.width || lastWindowSize.y != Screen.height)
        {
            CalculateRect();
        }
    }

    // Update is called once per frame
    void OnGUI()
    {
        GUI.DrawTexture(screenPos, crossHair);
    }
    
    void CalculateRect()
    {
        screenPos = new Rect((Screen.width - crossHair.width * scaleFactor) / 2, (Screen.height - crossHair.height * scaleFactor) / 2, crossHair.width * scaleFactor, crossHair.height * scaleFactor);
        lastWindowSize.x = Screen.width;
        lastWindowSize.y = Screen.height;
    }

}
