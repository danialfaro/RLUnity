using System.IO;
using UnityEngine;

public struct MazeData
{
    public (int, int) size;
    public int[,] maze;
}
class MazeLoader
{
    public static MazeData ReadFile(string filename)
    {
        string fileName = @"C:\Users\MSI\Desktop\maze-runner\" + filename;

        // Comprobamos que el archivo existe antes de intentar leerlo
        if (File.Exists(fileName))
        {
            MazeData mazeData = new MazeData();

            using (StreamReader reader = new StreamReader(fileName))
            {
                int lineCount = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] splited = line.Trim().Split(' ');

                    if (lineCount == 0)
                    {
                        (int, int) s = (int.Parse(splited[0]), int.Parse(splited[1]));
                        mazeData.size = s;
                        mazeData.maze = new int[s.Item1, s.Item2];
                    }
                    else
                    {
                        for (int i = 0; i < splited.Length; i++)
                        {
                            mazeData.maze[lineCount - 1, i] = int.Parse(splited[i]);
                        }
                    }

                    lineCount++;
                }
            }

            return mazeData;
        }
        else
        {
            Debug.Log("El archivo no existe");

            MazeData defaultMaze = new MazeData();
            defaultMaze.size = (10, 10);
            defaultMaze.maze = new int[10, 10] {
                {1, 0, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 0, 1, 0, 1, 0, 0, 0, 0, 1},
                {1, 0, 1, 0, 1, 0, 1, 1, 0, 1},
                {1, 0, 1, 0, 0, 0, 0, 0, 0, 1},
                {1, 0, 1, 0, 1, 1, 1, 1, 0, 1},
                {1, 0, 1, 0, 1, 0, 0, 0, 0, 1},
                {1, 0, 1, 0, 1, 0, 1, 1, 0, 1},
                {1, 0, 1, 0, 1, 0, 1, 1, 0, 1},
                {1, 0, 0, 0, 0, 0, 1, 0, 0, 1},
                {1, 1, 1, 1, 1, 1, 1, 0, 1, 1}
            };

            return defaultMaze;
        }
    }
}
