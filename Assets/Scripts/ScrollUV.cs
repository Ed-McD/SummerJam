using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ScrollUV : MonoBehaviour {

    public List<Vector2> scrollingDirs = new List<Vector2>();
    public List<Vector2> offsets = new List<Vector2>();
    Renderer rend;
    public float speed;


    // Use this for initialization
    void Awake()
    {
        rend = GetComponent<Renderer>();
        for (int i = scrollingDirs.Count; i < rend.sharedMaterials.Length; i++)
        {
            scrollingDirs.Add(Vector2.zero);
        }
        for (int i = offsets.Count; i <scrollingDirs.Count; i ++)
        {
            offsets.Add(rend.sharedMaterials[i].mainTextureOffset);
        }
        Debug.Log(rend.sharedMaterials[1]);

    }

    // Update is called once per frame
    void Update()
    {      
        for (int i = 0; i < rend.sharedMaterials.Length; i++)
        {
            offsets[i] += scrollingDirs[i] * Time.deltaTime * speed;
            rend.sharedMaterials[i].mainTextureOffset = offsets[i];
        }  
    }
}

