using System.Collections.Generic;
using UnityEngine;

public class Agent
{
    public List<((int, int), float)> stateHistory; // state, reward
    private double alpha;
    private double randomFactor; // 80% explore, 20% exploit
    public Dictionary<(int, int), double> G;

    public Agent(int[,] states, double alpha = 0.15, double random_factor = 0.8)
    {
        stateHistory = new List<((int, int), float)>() { ((0, 0), 0) };
        this.alpha = alpha;
        this.randomFactor = random_factor;
        G = new Dictionary<(int, int), double>();
        InitReward(states);
    }

    private void InitReward(int[,] states)
    {
        for (int i = 0; i < states.GetLength(0); i++)
        {
            for (int j = 0; j < states.GetLength(1); j++)
            {
                if (states[i, j] == 1)
                {
                    G[(i, j)] = 0;
                }
                else
                {
                    G[(i, j)] = new System.Random().NextDouble() * (1.0 - 0.1) + 0.1;
                }
            }
        }
    }

    public string ChooseAction((int, int) state, List<string> allowedMoves)
    {
        double maxG = -10e15;
        string next_move = null;
        double randomN = new System.Random().NextDouble();
        if (randomN < randomFactor)
        {
            // if random number below random factor, choose random action
            next_move = allowedMoves[new System.Random().Next(allowedMoves.Count)];
        }
        else
        {
            // if exploiting, gather all possible actions and choose one with the highest G (reward)
            foreach (string action in allowedMoves)
            {
                (int, int) new_state = (state.Item1 + Maze.ACTIONS[action].Item1, state.Item2 + Maze.ACTIONS[action].Item2);
                if (G[new_state] >= maxG)
                {
                    next_move = action;
                    maxG = G[new_state];
                }
            }
        }

        return next_move;
    }

    public void UpdateStateHistory((int, int) state, float reward)
    {
        stateHistory.Add((state, reward));
    }

    public void Learn()
    {
        double target = 0;

        for (int i = stateHistory.Count - 1; i > 0; i--)
        {
            ((int, int), float) stateReward = stateHistory[i];
            (int, int) prev = stateReward.Item1;
            float reward = stateReward.Item2;
            G[prev] = G[prev] + alpha * (target - G[prev]);
            target += reward;
        }

        stateHistory.Clear();

        randomFactor -= 10e-5; // decrease random factor each episode of play
    }

    public void printG()
    {
        foreach (KeyValuePair<(int, int), double> entry in G)
        {
            Debug.Log(entry.Key + " : " + entry.Value);
        }
    }
}