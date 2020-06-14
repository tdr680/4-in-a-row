using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player
{
    A,  // human
    B,  // ai
    x
}

public class GameManager : MonoBehaviour
{
    public int numberOfRows;
    public int numberOfColumns;
    public int xInRow;
    public bool paused;

    public Column[] columns;

    [SerializeField]
    private Player player;

    void Start()
    {
        System.Random r = new System.Random(); 
        player =  player = (Player)(r.Next() % 2);

        paused = false;

        foreach (Column column in columns)
            column.onMouseDownEvent += OnColumnMouseDown;

        if (player == Player.B)
            StartCoroutine(aiMove(0.5f));
    }

    void OnColumnMouseDown(int index)
    {
        if (!paused)
            DropPiece(index);
    }

    void DropPiece(int index)
    {
        if (columns[index].DropPiece(player))
        {
            if (WinningMove())
                GameOver();
            else
                NextMove();
        }
        else
        {
            // move not possible
        }
    }

    bool WinningMove()
    {
        Board board = new Board(numberOfRows, numberOfColumns);
        board.LoadGrid(columns);
        return board.WinningMove(player, xInRow);
    }

    int Score()
    {
        int score = 0;
        /*
        Board board = new Board(numberOfRows, numberOfColumns);
        board.LoadGrid(columns);
        board.DropPiece(3, Player.A);
        print(board);
        */
        return score;
    }

    void GameOver()
    {
        foreach (Column column in columns)
            column.onMouseDownEvent -= OnColumnMouseDown;

        // current player won
        print("*** " + player + " ***");

        // reset game
        // destroy pieceGameObjects for every column
    }

    void NextMove()
    {
        player = (Player)(((int)player + 1) % 2);

        if (player == Player.B)
        {
            StartCoroutine(aiMove(2.0f));
            if (WinningMove())
                GameOver();
        }
    }

    IEnumerator aiMove(float thinking)
    {
        paused = true;
        yield return new WaitForSeconds(thinking);

        int[] cols = Enumerable.Range(0, numberOfColumns).ToArray();
        Utils.ShuffleArray(cols);
        print(String.Join(",", cols));
        for (int i = 0; i < cols.Length; i++)
        {
            if (columns[cols[i]].FreePiece() > -1)
            {
                DropPiece(cols[i]);
                paused = false;
                yield break;
            }
        }
        // no move possible
    }
}
