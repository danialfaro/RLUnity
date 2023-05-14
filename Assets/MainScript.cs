using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    private Maze maze;
    private Agent robot;
    private List<int> moveHistory = new List<int>();

    public Gradient gradient;
    public GameObject cube;

    MazeData mazeData;

    void Start()
    {
        mazeData = MazeLoader.ReadFile("maze20x20");

        maze = new Maze(mazeData);
        robot = new Agent(maze.maze, alpha: 0.01, random_factor: 0.45f);

        //StartCoroutine(StartTraining());
        StartTraining();
    }
    void StartTraining()
    {
        for (int i = 0; i < 15000; i++)
        {
            while (!maze.IsGameOver())
            {
                ((int, int) state, float _) = maze.GetStateAndReward();
                List<string> allowedMoves = maze.allowedStates[state];
                string action = robot.ChooseAction(state, allowedMoves);
                maze.UpdateMaze(action);
                ((int, int) newState, float reward) = maze.GetStateAndReward();
                robot.UpdateStateHistory(newState, reward);

                if (maze.steps > (maze.width * maze.height))
                {
                    maze.robotPosition = (maze.width - 2, maze.height - 3);
                }

            }

            robot.Learn();
            moveHistory.Add(maze.steps);
            maze = new Maze(mazeData);

            robot.printG();


            //yield return new WaitForSecondsRealtime(.1f);
        }

        Debug.Log("Move history: " + string.Join(", ", moveHistory));

        RenderMap();

        //yield return null;

    }
    void RenderMap()
    {
        foreach (KeyValuePair<(int, int), double> entry in robot.G)
        {
            int x = entry.Key.Item1;
            int y = entry.Key.Item2;
            GameObject c = Instantiate(cube, new Vector3(x, 0, y), Quaternion.identity);

            double[] flattenList = new List<double>(robot.G.Values).ToArray();
            double n = (entry.Value - flattenList.Min()) / (flattenList.Max() - flattenList.Min());

            //Debug.Log(entry.Key + " " + entry.Value);

            if (entry.Value == 0)
            {
                c.GetComponent<Renderer>().material.color = Color.black;
            }
            else
            {
                c.GetComponent<Renderer>().material.color = gradient.Evaluate((float)n);
                c.transform.localScale = new Vector3((float)n, c.transform.localScale.y, (float)n);
            }
        }
    }
}
