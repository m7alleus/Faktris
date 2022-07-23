using System.Collections;
using UnityEngine;

public class DifficultyManager : MonoBehaviour {

    public event System.Action DifficultyIncreased;

    [SerializeField]
    float secondsInterval = 10f;
    float secondsTimer;
    float moveSpeedDecrease = .05f;

    public float currentMoveInterval = .8f;
    float minMoveInterval = .3f;

    void Start() {
        secondsTimer = secondsInterval;
    }

    void Update() {
        secondsTimer -= Time.deltaTime;
        if (secondsTimer <= 0 && currentMoveInterval > minMoveInterval) {
            secondsTimer = secondsInterval;
            currentMoveInterval -= moveSpeedDecrease;
            if (DifficultyIncreased != null) {
                DifficultyIncreased();
            }
        }
    }
}