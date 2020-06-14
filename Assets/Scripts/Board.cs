using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Board
{
    int rows;
    int cols;

    Player[,] grid; 

    public Board(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
        InitGrid();
    }

    void InitGrid()
    {
        grid = new Player[rows, cols];
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                grid[r, c] = Player.x;
    }

    public void LoadGrid(Column[] columns)
    {
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                grid[r, c] = columns[c].pieces[r];
    }

    public int FreePiece(int index)
    {
        return -1;
    }

    public bool DropPiece(int index, Player player)
    {
        return false;
    }

    Player[] GetRow(int row)
    {
        return Enumerable.Range(0, grid.GetLength(1))
                .Select(x => grid[row, x])
                .ToArray();
    }

    Player[] GetCol(int col)
    {
        return Enumerable.Range(0, grid.GetLength(0))
                .Select(x => grid[x, col])
                .ToArray();
    }

    List<Player>[] GetDiagonals(bool bltr)
    {
        // init array of lists for diagonals
        List<Player>[] d = new List<Player>[rows + cols - 1];
        for (int i = 0; i < rows + cols - 1; i++)
            d[i] = new List<Player>();

        // fill diagonals
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                if (bltr) // bottom left, top right
                    d[r + c].Add(grid[r, c]);
                else      // top left, bottom right
                    d[c - r + rows - 1].Add(grid[r, c]);
        return d;
    }

    public bool WinningMove(Player player, int inRow)
    {
        // check columns
        for (int c = 0; c < cols; c++)
            if (InRow(GetCol(c), player, inRow))
                return true;

        // check rows
        for (int r = 0; r < rows; r++)
            if (InRow(GetRow(r), player, inRow))
                return true;

        // check diagonals (bottom left, top right)
        foreach (List<Player> d in GetDiagonals(true))
            if (InRow(d.ToArray(), player, inRow))
                return true;

        // check diagonals (top left, bottom right)
        foreach (List<Player> d in GetDiagonals(false))
            if (InRow(d.ToArray(), player, inRow))
                return true;

        return false;
    }

    bool InRow(Player[] pieces, Player player, int inRow)
    {
        int ir = 0;
        for (int i = 0; i < pieces.Length; i++)
            if (pieces[i] == player)
            {
                if (++ir == inRow)
                    return true;
            }
            else
                ir = 0;
        return false;
    }

    public override string ToString()
    {
        string o = "";
        for (int r = rows - 1; r >= 0; r--)
        {
            for (int c = 0; c < cols; c++)
            {
                o = o + grid[r, c];
                if (c < cols - 1)
                    o = o + "   ";
            }
            o = o + "\n";
        }
        return o;
    }
}
