using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManagerVersus : CharactersManager {

	public List<CharacterBehavior> charactersTeam1;
	public List<CharacterBehavior> charactersTeam2;
	public Transform team1Container;
	public Transform team2Container;

	public override void Init()
	{
		distance = -50;
		//Data.Instance.events.OnAlignAllCharacters += OnAlignAllCharacters;
		//Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
		//Data.Instance.events.OnReorderAvatarsByPosition += OnReorderAvatarsByPosition;
		Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
		Data.Instance.events.OnAvatarFall += OnAvatarFall;
		//Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
		//Data.Instance.events.OnAutomataCharacterDie += OnAutomataCharacterDie;

		Vector3 pos;

		float _y = 2;

		if (Data.Instance.isReplay)
			_y = 15;

		Vector3 posTeam1 = new Vector3(1, _y, distance);	 
		Vector3 posTeam2 = new Vector3(1, _y, distance*-1);	

		if (Data.Instance.multiplayerData.player1) { 
			CharacterBehavior cb = addCharacter(posTeam1, 0); 
			cb.team_for_versus = 1;
			cb.transform.SetParent (team1Container);
			cb.transform.localPosition = posTeam1;
			charactersTeam1.Add (cb);
			playerPositions.Add(0); 
		};
		if (Data.Instance.multiplayerData.player2) { 
			CharacterBehavior cb = addCharacter(posTeam2, 1); 
			cb.team_for_versus = 2;
			cb.transform.SetParent (team2Container);
			cb.transform.localPosition = posTeam1;
			charactersTeam2.Add (cb);
			playerPositions.Add(1); 
		};

	}
}
