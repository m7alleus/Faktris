using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public int width = 10;
    public int height = 20;

    public Transform[,] grid;

    void Start() {
        grid = new Transform[width, height];
    }

    void Update() {

    }

    public void Register(Block block) {
        foreach (Transform square in block.transform) {
            int x = Mathf.FloorToInt(square.transform.position.x);
            int y = Mathf.FloorToInt(square.transform.position.y);

            grid[x, y] = square.transform;
        }
        CheckForCompleteRows();
    }

    void CheckForCompleteRows() {
        // TODO: rewrite this 
        bool completeRowFlag = true;
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (!completeRowFlag) {
                    continue;
                }
                if (grid[x, y] == null) {
                    completeRowFlag = false;
                }
            }
            if (completeRowFlag) {
                DeleteRow(y);
                y--;
            } else {
                completeRowFlag = true;
            }
        }
    }

    void DeleteRow(int rowIndex) {
        for (int x = 0; x < width; x++) {
            Transform squareTransform = grid[x, rowIndex];
            grid[x, rowIndex] = null;
            Destroy(squareTransform.gameObject);
        }
        ShiftRowsDown(rowIndex + 1);
    }

    void ShiftRowsDown(int rowIndex) {
        for (int y = rowIndex; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (grid[x, y] != null) {
                    grid[x, y].position += Vector3.down;
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                }
            }
        }
    }
}
