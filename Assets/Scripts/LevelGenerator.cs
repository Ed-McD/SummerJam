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
    [SerializeField] float blockScale = 1;
    [SerializeField] int dataHeightDistanceMin = 5;
    [SerializeField] int dataHeightDistanceMax = 10;

    [Tooltip("Define chances of each block appearing")] 
    [SerializeField] int defaultChance = 5;
    [SerializeField] int permanentChance = 3;
    [SerializeField] int fragileChance = 2;
    [SerializeField] int holdsDataChance = 5;
    [SerializeField] int pillarChance = 4;
    [SerializeField] int pillarScale = 20;

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
        Destroy(block);
    }

    public void RemoveData(GameObject data)
    {
        Destroy(data);
    }

    public void RemoveLevel()
    {
        for(int i = 0; i < blocks.Count; ++i)
        {
            RemoveBlock(blocks[i]);
        }
        for(int i = 0; i < datas.Count; ++i)
        {
            RemoveData(datas[i]);
        }

        blocks.Clear();
        datas.Clear();
    }

    void CreateLevel()
    {
        for(int i = 0; i < levelSize; ++i)
        {
            for (int j = 0; j < levelSize; ++j)
            {
                GameObject block = Instantiate(blockPrefab, new Vector3(i * blockGap, Random.Range(-pillarScale, pillarScale + 1), j * blockGap), Quaternion.identity) as GameObject;
                block.transform.localScale *= blockScale;
                BlockBehaviour bBehave = block.AddComponent<BlockBehaviour>();

                int blockType = Random.Range(0, pillarChance);
                bBehave.Init(blockType, defaultChance, permanentChance, fragileChance, holdsDataChance, pillarChance, pillarScale);

                if(bBehave.blocktype == BlockType.HoldsData)
                {
                    GameObject data = Instantiate(dataPrefab) as GameObject;
                    //data.transform.parent = block.transform;     ///animations on data cubes breaks with this, dunno why. if this is only to tidy the hierarcchy then I've replaced with hideflags
                    data.hideFlags = HideFlags.HideInHierarchy;
                    data.transform.position = block.transform.position + new Vector3(0, Random.Range(- (float)dataHeightDistanceMin,-dataHeightDistanceMax ), 0);

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
        if (Input.GetButtonDown("Jump"))
        {
            RemoveLevel();
            CreateLevel();
        }
    }
}
