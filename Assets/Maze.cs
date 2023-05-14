using System.Collections.Generic;

public class Maze
{
    public int[,] maze;
    public Dictionary<(int, int), List<string>> allowedStates;
    public (int, int) robotPosition;
    public int steps;

    public static readonly Dictionary<string, (int, int)> ACTIONS = new Dictionary<string, (int, int)> {
        {"U", (0, -1)},
        {"D", (0, 1)},
        {"L", (-1, 0)},
        {"R", (1, 0)}
    };

    public int width = 10;
    public int height = 10;

    public Maze(MazeData mazeData)
    {
        width = mazeData.size.Item1;
        height = mazeData.size.Item2;
        maze = mazeData.maze;

        maze[0, 1] = 2;
        robotPosition = (0, 1);
        steps = 0;
        ConstructAllowedStates();

    }

    public bool IsAllowedMove((int, int) state, string action)
    {
        int x = state.Item1;
        int y = state.Item2;
        x += ACTIONS[action].Item1;
        y += ACTIONS[action].Item2;
        if (x < 0 || y < 0 || x > width - 1 || y > height - 1)
        {
            return false;
        }
        if (maze[x, y] == 0 || maze[x, y] == 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ConstructAllowedStates()
    {
        allowedStates = new Dictionary<(int, int), List<string>>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (maze[i, j] != 1)
                {
                    allowedStates[(i, j)] = new List<string>();
                    foreach (string action in ACTIONS.Keys)
                    {
                        if (IsAllowedMove((i, j), action) && action != "0")
                        {
                            allowedStates[(i, j)].Add(action);
                        }
                    }
                }
            }
        }
    }

    public void UpdateMaze(string action)
    {
        int x = robotPosition.Item1;
        int y = robotPosition.Item2;
        maze[x, y] = 0;
        x += ACTIONS[action].Item1;
        y += ACTIONS[action].Item2;
        robotPosition = (x, y);
        maze[x, y] = 2;
        steps++;
    }

    public bool IsGameOver()
    {
        if (robotPosition == (width - 2, height - 3))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public ((int, int), int) GetStateAndReward()
    {
        return (robotPosition, GiveReward());
    }

    public int GiveReward()
    {
        //if at end give 0 reward
        //if not at end give -1 reward
        if (robotPosition == (width - 2, height - 3))
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }
}