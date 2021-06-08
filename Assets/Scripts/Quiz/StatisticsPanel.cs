using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatisticsPanel : MonoBehaviour
{
    public PlayerStatisticsPanel[] playerStatisticsPanels;

    private void Start()
    {
        Dictionary<string, GameStatistics.PlayerStatistics> statistics;
        if (GameStatistics.IsReady)
        {
            statistics = GameStatistics.Instance.AllPlayerStatistics;
        }
        else
        {
            statistics = new Dictionary<string, GameStatistics.PlayerStatistics>();
        }

        int maxScore = 10;
        foreach (GameStatistics.PlayerStatistics playerStatistics in statistics.Values)
        {
            int score = playerStatistics.Score;
            if (score > maxScore)
            {
                maxScore = score;
            }
        }
        Debug.LogFormat("Max Score: {0}", maxScore);

        int i = 0;
        foreach (GameStatistics.PlayerStatistics playerStatistics in statistics.Values)
        {
            Debug.LogFormat("Stats: {0} -> {1}", playerStatistics.name, playerStatistics.Score);

            if (i >= playerStatisticsPanels.Length) break;

            playerStatisticsPanels[i].UpdateStatistics(playerStatistics.name, playerStatistics.Score, maxScore);

            i++;
        }

        for (;i < playerStatisticsPanels.Length; i += 1)
        {
            playerStatisticsPanels[i].gameObject.SetActive(false);
        }

        return;
    }

    public void Leave()
    {
        SceneManager.LoadScene(0);
    }
}
