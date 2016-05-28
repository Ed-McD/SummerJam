using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ScrollUV : MonoBehaviour {

    public Vector2 scrollingDir = new Vector2(1.0f,0.0f);
    Vector2 offset = Vector2.zero;
    Renderer rend;
    public float speed;
    Mesh mesh;

    // Use this for initialization
    void Awake()
    {
        rend = GetComponent<Renderer>();
        mesh = GetComponent<Mesh>();

    }

    // Update is called once per frame
    void Update()
    {
        offset += scrollingDir * Time.deltaTime * speed;
        if(rend != null)
        {
            rend.sharedMaterial.mainTextureOffset = offset;
        }
    }
}
