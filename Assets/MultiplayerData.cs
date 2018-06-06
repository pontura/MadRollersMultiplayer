using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerData : MonoBehaviour
{
    public float newVictoryAreaScore;
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
	public void SelectAllPlayers()
	{
		player1 = true;
		player2 = true;
		player3 = true;
		player4 = true;
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
	public int GetTotalScore()
	{
		return score_player1 + score_player2 + score_player3 + score_player4;
	}
	public int GetPositionByScore(int _playerID)
	{
		int myScore = score_player1;
		if (_playerID == 1)
			myScore = score_player2;
		else if (_playerID == 2)
			myScore = score_player3;
		else if (_playerID == 3)
			myScore = score_player4;
		
		int puesto = 1;
		if (myScore < score_player1)
			puesto++;
		if (myScore < score_player2)
			puesto++;
		if (myScore < score_player3)
			puesto++;
		if (myScore < score_player4)
			puesto++;

		return puesto;
	}
}
