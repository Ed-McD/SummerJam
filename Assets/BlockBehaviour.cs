using UnityEngine;
using System.Collections;

public enum BlockType
{
    Default,
    Permanent,
    Fragile,
    HoldsData
}

public class BlockBehaviour : MonoBehaviour
{
    public float blockSize = 1;
    public BlockType blocktype;

    public void Init(int i, int defaultChance, int permanentChance, int fragileChance, int holdsDataChance)
    {
        if (i < defaultChance)
            blocktype = BlockType.Default;
        else if (i < permanentChance)
            blocktype = BlockType.Permanent;
        else if (i < fragileChance)
            blocktype = BlockType.Fragile;
        else if (i < holdsDataChance)
            blocktype = BlockType.HoldsData;

        MeshRenderer mRenderer = GetComponent<MeshRenderer>();

        switch(blocktype)
        {
            case BlockType.Default:
                mRenderer.material = LevelGenerator.instance.blockMaterials[0];
                break;
            case BlockType.Permanent:
                mRenderer.material = LevelGenerator.instance.blockMaterials[1];
                break;
            case BlockType.Fragile:
                mRenderer.material = LevelGenerator.instance.blockMaterials[2];
                break;
            case BlockType.HoldsData:
                mRenderer.material = LevelGenerator.instance.blockMaterials[3];
                break;

        }

        print(blocktype);
    }

	
	// Update is called once per frame
	void Update ()
    {
	    
	}



}
