using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCreator : MonoBehaviour
{
    public int mazeWidth;
    public int mazeHeight;
    public Location mazeStart = new Location(0, 0);

    Grid maze;
    GameObject wallPrefab;

    void Start()
    {
        // Initialize grid with maze dimensions
        maze = new Grid(mazeWidth, mazeHeight);
        generateMaze(maze, mazeStart);

        // Load wall prefab in
        wallPrefab = Resources.Load<GameObject>("Wall");
        BuildMaze();
    }

    void Update()
    {
        // When user presses G reset maze
        if (Input.GetKeyDown(KeyCode.G))
        {
            // create array of gameObjects tagged Wall
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            Debug.Log("Found " + walls.Length + " walls.");

            // Destroy all previous walls
            foreach (GameObject wall in walls)
            {
                Destroy(wall);
            }

            // Generate a new maze within the dimensions below
            mazeWidth = (int)Random.Range(20, 30);
            mazeHeight = (int)Random.Range(20, 30);
            maze = new Grid(mazeWidth, mazeHeight);
            generateMaze(maze, mazeStart);
            BuildMaze();
        }       
    }

    // Build maze function
    void BuildMaze()
    {
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                Connections currentCell = maze.cells[x, y];
                if (maze.cells[x, y].inMaze)
                {
                    Vector3 cellPos = new Vector3(x, 0, y);
                    float lineLength = 1f;
                    if (!currentCell.directions[0])
                    {
                        Vector3 wallPos = new Vector3(x + lineLength / 2, 0, y);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.identity) as GameObject;
                    }
                    if (!currentCell.directions[1])
                    {
                        Vector3 wallPos = new Vector3(x, 0, y + lineLength / 2);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
                    }

                }
            }
        }
    }

    // Generate the maze
    void generateMaze(Level level, Location start)
    {
        Stack<Location> locations = new Stack<Location>();
        locations.Push(start);
        level.startAt(start);

        while (locations.Count > 0)
        {
            Location current = locations.Peek();

            Location next = level.makeConnection(current);
            if (next != null)
            {
                locations.Push(next);
            }
            else
            {
                locations.Pop();
            }
        }
    }
}