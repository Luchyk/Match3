using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject BackgroundPrephab;

    [SerializeField] private Fishka [] fishPrephab;
    public Fishka [,] allTiles;

    public float tileSpeed = 7f;

    private void Start()
    {
        allTiles = new Fishka [width, height];

        ClearBoard();
        
        SetUpBoard();
       
        StartCoroutine(RefillBoard());
    }

    private void ClearBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                allTiles[i, j] = null;
            }
        }
    }

    [ContextMenu("Set Up Cells")]
    private void SetUpBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPos = new Vector2(i, j);
                GameObject bgTile = Instantiate(BackgroundPrephab, tempPos, Quaternion.identity);
         
                bgTile.transform.parent = transform;
                bgTile.name = "BP - " + i + ", " + j;
            }
        }
    }

    private void SetUpFirstFishes()
    {
       StartCoroutine(DecreaseRowCol());        
    }
    
    [ContextMenu("Set Up Fishes")]
    private void SpawnFishes(Vector2Int pos, Fishka fishToSpawn)
    {
        Fishka fishka = Instantiate(fishToSpawn, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
        fishka.transform.parent = transform;
        fishka.name = "Fishka - " + pos.x + ", " + pos.y;
        allTiles[pos.x, pos.y] = fishka;
        fishka.SetUpFishes(pos, this);
    }

    private IEnumerator DecreaseRowCol()
    {
        yield return new WaitForSeconds(.0f);

        int nullCounter = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allTiles[x, y] == null)
                {
                    nullCounter++;
                }
                else if (nullCounter > 0)
                {
                    allTiles[x, y].posIndex.y -= nullCounter;
                    allTiles[x, y - nullCounter] = allTiles[x, y];
                    allTiles[x, y] = null;
                    
                    StartCoroutine(RefillBoard());
                }
            }
            nullCounter = 0;  
        }
    }

    private IEnumerator RefillBoard()
    {
        yield return new WaitForSeconds(.1f);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if ((allTiles[i, j] == null)&&(allTiles[i, 8] == null))
                { 
                    int fishToUse = Random.Range(0, fishPrephab.Length);
                    SpawnFishes(new Vector2Int(i,8), fishPrephab[fishToUse]);
                    StartCoroutine(DecreaseRowCol());
                }
            }           
        }
    }

    public void DestroyFishes(Vector2Int pos)
    {
        StartCoroutine(DecreaseRowCol());
    }
}
