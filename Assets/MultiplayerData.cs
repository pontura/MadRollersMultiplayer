using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerData : MonoBehaviour
{
    public float distance;

    public bool player1;
    public bool player2;
    public bool player3;
    public bool player4;

    public Color[] colors;

    public List<int> players;

    public int score_player1;
    public int score_player2;
    public int score_player3;
    public int score_player4;

    void Start()
    {
        Data.Instance.events.OnReorderAvatarsByPosition += OnReorderAvatarsByPosition;
    }
    void OnReorderAvatarsByPosition(List<int> _players)
    {
        players = _players;
    }
    public int GetScore(int playerID)
    {
        switch (playerID)
        {
            case 0: return score_player1;
            case 1: return score_player2;
            case 2: return score_player3;
            default: return score_player4;
        }
    }
}
