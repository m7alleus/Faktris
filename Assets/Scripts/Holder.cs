using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour {

    public bool canHold;
    Spawner spawner;
    Block currentBlock;

    void Start() {
        canHold = true;
        spawner = FindObjectOfType<Spawner>();
    }

    void Update() {
        if (Input.GetKey(KeyCode.C) && canHold && spawner.currentBlock != null) {
            Hold();
        }
    }

    void Hold() {
        canHold = false;
        Block spawnerCurrentBlock = spawner.currentBlock;
        if (currentBlock == null) {
            PlaceBlockOnHold(spawnerCurrentBlock);
            spawner.MarkAsReadyToSpawn();
            spawner.Spawn();
        } else {
            ReleaseBlock();
            PlaceBlockOnHold(spawnerCurrentBlock);
        }
        spawner.currentBlock.IsStill += MarkAsReadyToHold;
    }

    void PlaceBlockOnHold(Block block) {
        currentBlock = block;
        currentBlock.currentState = Block.State.Inactive;
        currentBlock.transform.position = transform.position;
    }

    void ReleaseBlock() {
        spawner.TriggerBlock(currentBlock);
        currentBlock = null;
    }

    void MarkAsReadyToHold() {
        canHold = true;
    }
}
