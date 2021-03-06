﻿using UnityEngine;
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

    bool hitCollider = false;

    public void Init(int i, int defaultChance, int permanentChance, int fragileChance, int holdsDataChance, int pillarChance, int pillarScale)
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
        Animator mAnim = GetComponent<Animator>();

        int random = Random.Range(0,2);
        bool pillarUp = random == 1 ? true : false;

        switch(blocktype)
        {
            case BlockType.Default:
                mRenderer.material = LevelGenerator.instance.blockMaterials[0];
                blockLifetime = 3.0f;
                mAnim.SetInteger("colourType", 1);
                GetComponent<ReCalcCubeTexture>().enabled = false;
                break;
            case BlockType.Permanent:
                mRenderer.material = LevelGenerator.instance.blockMaterials[1];
                mAnim.SetInteger("colourType", 0);
                GetComponent<ReCalcCubeTexture>().enabled = false;
                break;
            case BlockType.Fragile:
                mRenderer.material = LevelGenerator.instance.blockMaterials[2];
                mAnim.SetInteger("colourType", 4);
                GetComponent<ReCalcCubeTexture>().enabled = false;
                break;
            case BlockType.HoldsData:
                mRenderer.material = LevelGenerator.instance.blockMaterials[3];
                blockLifetime = 3.0f;
                mAnim.SetInteger("colourType", 5);
                GetComponent<ReCalcCubeTexture>().enabled = false;
                break;
            case BlockType.Pillar:
                
                transform.localScale += new Vector3(0, pillarUp ? pillarScale : -pillarScale, 0);
                transform.localPosition -= new Vector3(0, (pillarUp ? pillarScale : -pillarScale )/ 2, 0);
                GetComponent<ReCalcCubeTexture>().Calculate();
                mRenderer.material = LevelGenerator.instance.blockMaterials[4];
                GetComponent<BoxCollider>().isTrigger = false;
                mAnim.SetInteger("colourType", 2);
                GetComponent<ReCalcCubeTexture>().enabled = true;
                break;

        }

        //GetComponent<ReCalcCubeTexture>().Calculate();
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

    void SetDropBlock()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints &= ~RigidbodyConstraints.FreezeAll;
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "BottomCollider" && !hitCollider)
        {
            hitCollider = true;
            LevelGenerator.instance.RemoveBlock(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

	}



}
