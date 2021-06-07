using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatistics : MonoBehaviour
{
    private static GameStatistics _instance;
    public static GameStatistics Instance { get { return _instance; } }
    public static bool IsReady { get { return _instance != null; } }

    private Dictionary<string, PlayerStatistics> playerStatistics;

    public class PlayerStatistics
    {
        public string name;
        public float talkDuration = 0f;

        public PlayerStatistics(string name)
        {
            this.name = name;
        }
    }

    private void Awake()
    {
        _instance = this;
        playerStatistics = new Dictionary<string, PlayerStatistics>();
    }

    public void UpdateTalkDuration(string name, float talkDuration)
    {
        getPlayerStatisctics(name).talkDuration = talkDuration;
    }

    public PlayerStatistics getPlayerStatisctics(string name)
    {
        if (!playerStatistics.ContainsKey(name))
        {
            playerStatistics.Add(name, new PlayerStatistics(name));
        }

        print(playerStatistics[name]);

        return playerStatistics[name];
    }
}
