using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : Level
{

    Vector3[] NEIGHBORS = new Vector3[4]
    {
        new Vector3(1, 0, 0),
        new Vector3(0, 1, 1),
        new Vector3(0, -1, 2),
        new Vector3(-1, 0, 3)
    };

    int m_Width, m_Height;
    public Connections[,] cells;

    public Grid(int width, int height)
    {
        m_Width = width;
        m_Height = height;
        cells = new Connections[width, height];

        for (int i = 0; i < m_Width; i++)
        {
            for (int j = 0; j < m_Height; j++)
            {
                cells[i, j] = new Connections();
            }
        }
    }

    public override void startAt(Location location)
    {
        cells[location.x, location.y].inMaze = true;
    }

    bool canPlaceCorridor(int x, int y, int dirn)
    {
        return (x >= 0 && x < m_Width) && (y >= 0 && y < m_Height) && !cells[x, y].inMaze;
    }

    void shuffleNeighbors()
    {
        int n = NEIGHBORS.Length;
        while (n > 1)
        {
            n--;
            int k = (int)Random.Range(0, n);
            Vector3 v = NEIGHBORS[k];
            NEIGHBORS[k] = NEIGHBORS[n];
            NEIGHBORS[n] = v;
        }
    }

    public override Location makeConnection(Location location)
    {
        shuffleNeighbors();

        int x = location.x;
        int y = location.y;
        foreach (Vector3 v in NEIGHBORS)
        {
            int dx = (int)v.x;
            int dy = (int)v.y;
            int dirn = (int)v.z;

            int nx = x + dx;
            int ny = y + dy;
            int fromDirn = 3 - dirn;
            if (canPlaceCorridor(nx, ny, fromDirn))
            {
                cells[x, y].directions[dirn] = true;
                cells[nx, ny].inMaze = true;
                cells[nx, ny].directions[fromDirn] = true;
                return new Location(nx, ny);
            }
        }
        return null;
    }
}