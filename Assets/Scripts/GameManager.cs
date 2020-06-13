using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player
{
    A,
    B,
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
        player = Player.A;
        paused = false;

        foreach (Column column in columns)
            column.onMouseDownEvent += OnColumnMouseDown;
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
            //DumpBoard();
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
        // check columns
        foreach (Column c in columns)
            if (InRow(c.pieces))
                return true;

        // check rows
        for (int r = 0; r < numberOfRows; r++)
            if (InRow(GetRow(r)))
                return true;

        // check diagonals (bottom left, top right)
        foreach (List<Player> d in GetDiagonals(true))
            if (InRow(d.ToArray()))
                return true;

        // check diagonals (top left, bottom right)
        foreach (List<Player> d in GetDiagonals(false))
            if (InRow(d.ToArray()))
                return true;

        return false;
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
            StartCoroutine(aiMove());
    }

    IEnumerator aiMove()
    {
        paused = true;
        yield return new WaitForSeconds(2); // pretending i am thinking...

        int[] cols = Enumerable.Range(0, numberOfColumns).ToArray();
        shuffle(cols);
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

    bool InRow(Player[] pieces)
    {
        int inRow = 0;
        for (int i = 0; i < pieces.Length; i++)
            if (pieces[i] == player)
            {
                if (++inRow == xInRow)
                    return true;
            }
            else
                inRow = 0;
        return false;
    }

    Player[] GetRow(int row)
    {
        Player[] pieces = new Player[numberOfColumns];
        for (int c = 0; c < numberOfColumns; c++)
            pieces[c] = Player.x;

        for (int c = 0; c < numberOfColumns; c++)
            pieces[c] = columns[c].pieces[row];
        return pieces;
    }

    List<Player>[] GetDiagonals(bool bltr)
    {
        // init array of lists for diagonals
        List<Player>[] d = new List<Player>[numberOfRows + numberOfColumns - 1];
        for (int i = 0; i < numberOfRows + numberOfColumns - 1; i++)
            d[i] = new List<Player>();

        // fill diagonals
        for (int r = 0; r < numberOfRows; r++)
            for (int c = 0; c < numberOfColumns; c++)
                if (bltr) // bottom left, top right
                    d[r + c].Add(columns[c].pieces[r]);
                else      // top left, bottom right
                    d[c - r + numberOfRows - 1].Add(columns[c].pieces[r]);
        return d;
    }

    void shuffle(int []arr) 
    { 
        System.Random r = new System.Random(); 
          
        // Start from the last element and 
        // swap one by one. We don't need to 
        // run for the first element  
        // that's why i > 0 
        for (int i = arr.Length - 1; i > 0; i--)  
        {
            // Pick a random index 
            // from 0 to i 
            int j = r.Next(0, i+1); 
              
            // Swap arr[i] with the 
            // element at random index 
            int temp = arr[i]; 
            arr[i] = arr[j]; 
            arr[j] = temp; 
        } 
    }

    void DumpBoard()
    {
        string o = "";
        for (int r = numberOfRows - 1; r >= 0; r--)
        {
            for (int c = 0; c < numberOfColumns; c++)
            {
                o = o + columns[c].pieces[r] + "   ";
            }
            o = o + "\n";
        }
        print(o);
    }
}
