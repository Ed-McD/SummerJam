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

    [Tooltip("Define chances of each block appearing")] 
    [SerializeField] int defaultChance = 5;
    [SerializeField] int permanentChance = 3;
    [SerializeField] int fragileChance = 2;
    [SerializeField] int holdsDataChance = 5;

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

        blockParent = new GameObject("BlockParent");
        blockParent.transform.parent = gameObject.transform;

        CreateLevel();

    }

    void CreateLevel()
    {
        for(int i = 0; i < levelSize; ++i)
        {
            for (int j = 0; j < levelSize; ++j)
            {
                GameObject block = Instantiate(blockPrefab, new Vector3(i * blockGap, gameObject.transform.position.y, j * blockGap), Quaternion.identity) as GameObject;
                BlockBehaviour bBehave = block.AddComponent<BlockBehaviour>();

                int blockType = Random.Range(0, holdsDataChance);
                bBehave.Init(blockType, defaultChance, permanentChance, fragileChance, holdsDataChance);

                if(bBehave.blocktype == BlockType.HoldsData)
                {
                    GameObject data = Instantiate(dataPrefab) as GameObject;
                    data.transform.parent = block.transform;
                    data.transform.position = block.transform.position + new Vector3(0, -2, 0);

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
