using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    static LevelGenerator _instance;
    public static LevelGenerator instance {  get { return _instance; } }

    [SerializeField] GameObject blockPrefab = null;
    [SerializeField] GameObject dataPrefab = null;
    public float levelSize = 5;
    public float blockGap = 3;
    [SerializeField] int dataHeightDistance = 5;

    [Tooltip("Define chances of each block appearing")] 
    [SerializeField] int defaultChance = 5;
    [SerializeField] int permanentChance = 3;
    [SerializeField] int fragileChance = 2;
    [SerializeField] int holdsDataChance = 5;
    [SerializeField] int pillarChance = 4;

    [Tooltip("Ensure the materials are in the same order as the chances")]
    public Material[] blockMaterials;

    List<GameObject> blocks = new List<GameObject>();
    List<GameObject> datas = new List<GameObject>();
    GameObject blockParent = null;

	// Use this for initialization
	void Start ()
    {
        _instance = this;

        permanentChance = defaultChance + permanentChance;
        fragileChance = permanentChance + fragileChance;
        holdsDataChance = fragileChance + holdsDataChance;
        pillarChance = holdsDataChance + pillarChance;

        blockParent = new GameObject("BlockParent");
        blockParent.transform.parent = gameObject.transform;

        CreateLevel();

    }

    public void RemoveBlock(GameObject block)
    {
        blocks.Remove(block);
    }

    public void RemoveData(GameObject data)
    {
        datas.Remove(data);
    }

    void CreateLevel()
    {
        for(int i = 0; i < levelSize; ++i)
        {
            for (int j = 0; j < levelSize; ++j)
            {
                GameObject block = Instantiate(blockPrefab, new Vector3(i * blockGap, gameObject.transform.position.y, j * blockGap), Quaternion.identity) as GameObject;
                BlockBehaviour bBehave = block.AddComponent<BlockBehaviour>();

                int blockType = Random.Range(0, pillarChance);
                bBehave.Init(blockType, defaultChance, permanentChance, fragileChance, holdsDataChance, pillarChance);

                if(bBehave.blocktype == BlockType.HoldsData)
                {
                    GameObject data = Instantiate(dataPrefab) as GameObject;
                    data.transform.parent = block.transform;
                    data.transform.position = block.transform.position + new Vector3(0, -dataHeightDistance, 0);

                    datas.Add(data);
                }

                block.transform.parent = blockParent.transform;
                blocks.Add(block);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
