using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour {

    public void PlaceBlockOnHold(Block block) {
        //block.currentState = Block.State.Inactive;
        block.SetAsInactive();
        block.transform.position = transform.position;
    }
}
