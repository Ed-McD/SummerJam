using UnityEngine;
using System.Collections;

public enum BlockType
{
    Default,
    Permanent,
    Fragile,
    HoldsData,
    Pillar
}

public class BlockBehaviour : MonoBehaviour
{
    public BlockType blocktype;
    float blockLifetime = 0;
    bool dropBlock = false;

    public void Init(int i, int defaultChance, int permanentChance, int fragileChance, int holdsDataChance, int pillarChance)
    {
        if (i < defaultChance)
            blocktype = BlockType.Default;
        else if (i < permanentChance)
            blocktype = BlockType.Permanent;
        else if (i < fragileChance)
            blocktype = BlockType.Fragile;
        else if (i < holdsDataChance)
            blocktype = BlockType.HoldsData;
        else if (i < pillarChance)
            blocktype = BlockType.Pillar;

        MeshRenderer mRenderer = GetComponent<MeshRenderer>();

        switch(blocktype)
        {
            case BlockType.Default:
                mRenderer.material = LevelGenerator.instance.blockMaterials[0];
                blockLifetime = 3.0f;
                break;
            case BlockType.Permanent:
                mRenderer.material = LevelGenerator.instance.blockMaterials[1];
                break;
            case BlockType.Fragile:
                mRenderer.material = LevelGenerator.instance.blockMaterials[2];
                break;
            case BlockType.HoldsData:
                mRenderer.material = LevelGenerator.instance.blockMaterials[3];
                blockLifetime = 3.0f;
                break;
            case BlockType.Pillar:
                mRenderer.material = LevelGenerator.instance.blockMaterials[4];
                transform.localScale += new Vector3(0, 10, 0);
                transform.localPosition -= new Vector3(0, 5, 0);
                break;

        }


        print(blocktype);
    }


    public void HitBlock()
    {
        switch (blocktype)
        {
            case BlockType.Default:
                Invoke("SetDropBlock", blockLifetime);
                break;
            case BlockType.Permanent:
                // do nothing
                break;
            case BlockType.Fragile:
                SetDropBlock();
                break;
            case BlockType.HoldsData:
                Invoke("SetDropBlock", blockLifetime);
                break;

        }
    }

    void RemoveBlock()
    {
        LevelGenerator.instance.RemoveBlock(gameObject);
        DestroyImmediate(gameObject);
    }

    void SetDropBlock()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
    }
	
	// Update is called once per frame
	void Update ()
    {

	}



}
