using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SummaryMultiplayer : MonoBehaviour {

    public GameObject title;
    public SummaryArcadePlayerUI player1;
    public SummaryArcadePlayerUI player2;
    public SummaryArcadePlayerUI player3;
    public SummaryArcadePlayerUI player4;

    private MultiplayerData multiplayerData;

    public states state;
    public enum states
    {
        INTRO,
        READY,
        DONE
    }

    private IEnumerator nextRoutine;

	void Start () {
        Time.timeScale = 1;
        multiplayerData = Data.Instance.multiplayerData;

        MultiplayerSummaryTexts texts = Data.Instance.GetComponent<MultiplayerSummaryTexts>();

        for (int playerID = 0; playerID < 4; playerID++)
            GetPosition(playerID);

        int player_position_1 = multiplayerData.players[0];
        int player_position_2 = multiplayerData.players[1];
        int player_position_3 = multiplayerData.players[2];
        int player_position_4 = multiplayerData.players[3];

        int score1 = multiplayerData.GetScore(player_position_1);
        int score2 = multiplayerData.GetScore(player_position_2);
        int score3 = multiplayerData.GetScore(player_position_3);
        int score4 = multiplayerData.GetScore(player_position_4);

        string title_1 = texts.GetText(1, score1);
        string title_2 = texts.GetText(2, score2);
        string title_3 = texts.GetText(3, score3);
        string title_4 = texts.GetText(4, score4);
        int totalScore = 1+ score1 + score2 + score3 + score4;

        player1.Init(multiplayerData.colors[player_position_1], title_1, score1, (score1 * 100) / totalScore, 1);
        player2.Init(multiplayerData.colors[player_position_2], title_2, score2, (score2 * 100) / totalScore, 2);
        player3.Init(multiplayerData.colors[player_position_3], title_3, score3, (score3 * 100) / totalScore, 3);
        player4.Init(multiplayerData.colors[player_position_4], title_4, score4, (score4 * 100) / totalScore, 4);

       
        foreach (Text field in title.GetComponentsInChildren<Text>())
            field.text = "Total = " + totalScore + "x en " + (int)multiplayerData.distance  + " mts.";

        nextRoutine = Next();
        StartCoroutine(nextRoutine);
	}
    IEnumerator Next()
    {
        yield return new WaitForSeconds(3);
        state = states.READY;
        yield return new WaitForSeconds(14);
        GotoIntro();
    }
    void Update()
    {
        if (state == states.READY)
        {
            if (InputManager.getFire(0) || InputManager.getJump(0) || InputManager.getFire(1) || InputManager.getJump(1)
                || InputManager.getFire(2) || InputManager.getJump(2) || InputManager.getFire(3) || InputManager.getJump(3))
            {           
                Reset();
                Data.Instance.LoadLevel("MainMenuArcade");
                state = states.DONE;
            }
        }
    }
    void Reset()
    {
        StopCoroutine(nextRoutine);
    }
    void GotoIntro()
    {
        Reset();
        if (state == states.READY)
            Data.Instance.LoadLevel("MainMenuArcade");
    }
    //si no está registrado lo agrega a la lista:
    private int GetPosition(int _playerID)
    {
        int id = 1;
        foreach(int playerID in multiplayerData.players)
        {
            if (playerID == _playerID)
                return id;
        }
        multiplayerData.players.Add(_playerID);
        return multiplayerData.players.Count;
    }
	
}
