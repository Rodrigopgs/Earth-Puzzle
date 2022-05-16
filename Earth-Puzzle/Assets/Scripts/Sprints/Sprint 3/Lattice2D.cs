using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// A 2D 'lattice'
/// </summary>
/// <typeparam name="TCell"></typeparam>
public class Lattice2D<TCell> : IEnumerable<TCell>
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public string name = "Lattice2D";

    public TCell[,] Cells { get; private set; }

    public Vector2 Origin { get; private set; }

    public Lattice2D(int width, int height)
    {
        Width = width;
        Height = height;
        Cells = new TCell[Width, Height];

        Origin = Vector2.zero;
    }

    public Lattice2D(int width, int height, Vector2 origin)
    {
        Width = width;
        Height = height;
        Cells = new TCell[Width, Height];

        Origin = origin;
    }

    public IEnumerator<TCell> GetEnumerator()
    {
        for (int w = 0; w < Width; w++)
        {
            for (int h = 0; h < Height; h++)
            {
                yield return Cells[w, h];
            }
        }
    }

    internal Vector3 RoundToLatticeWorldPosition(Vector3 vector3)
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public TCell GetCell(int x, int y) => Cells[x, y];
    public TCell GetCell(Vector2Int pos) => Cells[pos.x, pos.y];
    public TCell GetPosition(TCell cell, out int x, out int y)
    {
        for (int xw = 0; xw < Width; xw++)
        {
            for (int yh = 0; yh < Height; yh++)
            {
                if (EqualityComparer<TCell>.Default.Equals(Cells[xw, yh], cell))
                {
                    x = xw;
                    y = yh;
                    return Cells[xw, yh];
                }
            }
        }

        throw new System.InvalidOperationException("The TCell " + cell + " was not found in the lattice!");
    }
    public bool Contains(TCell cell)
    {
        for (int xw = 0; xw < Width; xw++)
        {
            for (int yh = 0; yh < Height; yh++)
            {
                if (EqualityComparer<TCell>.Default.Equals(Cells[xw, yh], cell))
                    return true;
            }
        }
        return false;
    }

    public void ForEach(System.Action<TCell> action)
    {
        foreach (TCell cell in this)
            action(cell);
    }

    public Vector2Int WorldToLatticePosition(Vector2 worldPosition)
    {
        Vector2Int result = new Vector2Int(Mathf.FloorToInt(worldPosition.x - Origin.x), Mathf.FloorToInt(worldPosition.y - Origin.y));

        if (result.x < 0 || result.x > Width || result.y < 0 || result.y > Height)
            throw new System.IndexOutOfRangeException("The world position was not part of the lattice!");

        return result;

    }
    public Vector2 LatticeToWorldPosition(Vector2Int latticePosition)
    {
        if (latticePosition.x < 0 || latticePosition.x > Width || latticePosition.y < 0 || latticePosition.y > Height)
            throw new System.IndexOutOfRangeException("The lattice position was not part of the lattice!");

        return new Vector2(latticePosition.x + Origin.x + 0.5f, latticePosition.y + Origin.y + 0.5f);
    }
    public Vector2 LatticeToWorldPosition(int x, int y)
    {
        if (x < 0 || x > Width || y < 0 || y > Height)
            throw new System.IndexOutOfRangeException("The lattice position was not part of the lattice!");

        return new Vector2(x + Origin.x + 0.5f, y + Origin.y + 0.5f);
    }
    public static Vector2 RoundToLatticeWorldPosition(Vector2 worldPosition) => new Vector2(Mathf.Floor(worldPosition.x) + 0.5f, Mathf.Floor(worldPosition.y) + 0.5f);

    public bool TrySetCell(TCell cell, int x, int y)
    {
        if (!EqualityComparer<TCell>.Default.Equals(Cells[x, y], cell))
        {
            Cells[x, y] = cell;
            return true;
        }
        else
            return false;
    }

    public void SetCell(TCell cell, int x, int y)
    {
        Cells[x, y] = cell;
    }

    public override string ToString()
    {
        string result = "Origin: " + Origin + " Width: " + Width + " Height: " + Height + " containing (" + (Width * Height) + "):";

        foreach (TCell cell in Cells)
            result += "\n" + cell.ToString();

        return result;
    }
    public string QuickToString()
    {
        return "Origin: " + Origin + " Width: " + Width + " Height: " + Height + " containing: " + (Width * Height) + " objects";
    }

    public TCell this[int x, int y]
    {
        get
        {
            return Cells[x, y];
        }
        set
        {
            Cells[x, y] = value;
        }
    }
}
