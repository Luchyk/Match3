using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishka : MonoBehaviour
{
    public Vector2Int posIndex;
    [SerializeField] private Cell cell;

    public void Update()
    {
        if (Vector2.Distance(transform.position, posIndex) > .01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, cell.tileSpeed * Time.deltaTime); 
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
            cell.allTiles[posIndex.x, posIndex.y] = this;
        }
    }
    
    public void SetUpFishes(Vector2Int pos, Cell theCell)
    {
        posIndex = pos;
        cell = theCell;
    }

    void OnMouseDown()
    {
        Destroy(gameObject);   
        cell.DestroyFishes(posIndex);
    }
}
