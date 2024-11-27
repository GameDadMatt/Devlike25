using UnityEngine;
using UnityEngine.Tilemaps;
using Tetronimo;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }

    //Temp stuff
    public Tile basicTile;

    private float seconds = 0;

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
    }

    private void Start()
    {
        RandomTetronimo();
    }

    private void Update()
    {
        seconds += Time.deltaTime;
        if (seconds >= 3f)
        {
            tilemap.ClearAllTiles();
            RandomTetronimo();
            seconds = 0f;
        }
    }

    private void RandomTetronimo()
    {
        int size = 8; //Random.Range(3, 10);
        int complexity = Random.Range(0, 10);
        TetronimoData tetronimo = new TetronimoData(size, complexity, basicTile);
        SpawnTetronimo(tetronimo);
    }

    private void SpawnTetronimo(TetronimoData tetronimo)
    {
        Debug.Log("Size " + tetronimo.size + " | Complexity " + tetronimo.complexity);
        for(int i = 0; i < tetronimo.cells.Count; i++)
        {
            Vector3Int tilePosition = (Vector3Int)tetronimo.cells[i];
            tilemap.SetTile(tilePosition, tetronimo.tile);
        }
    }
}
