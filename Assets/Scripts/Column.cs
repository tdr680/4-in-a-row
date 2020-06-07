using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    public event Action<int> onMouseDownEvent;
    
    public int numberOfPieces = 6;
    public Player[] pieces;

    public GameObject bluePiece;
    public GameObject redPiece;

    public List<GameObject> pieceGameObjects;

    public int index;

    void Start()
    {
        pieces = new Player[numberOfPieces];
        for (int i = 0; i < numberOfPieces; i++)
            pieces[i] = Player.x;

        pieceGameObjects = new List<GameObject>();
    }

    public bool DropPiece(Player player)
    {
        for (int i = 0; i < numberOfPieces; i++)
        {
            if (pieces[i] == Player.x)
            {
                pieces[i] = player;

                if (player == Player.A)
                    pieceGameObjects.Add(Instantiate(bluePiece, transform.position + (numberOfPieces + 1) * Vector3.up, Quaternion.identity));
                else
                    pieceGameObjects.Add(Instantiate(redPiece, transform.position + (numberOfPieces + 1) * Vector3.up, Quaternion.identity));

                return true;
            }
        }
        return false;
    }

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (onMouseDownEvent != null)
            onMouseDownEvent(index);
    }
}
