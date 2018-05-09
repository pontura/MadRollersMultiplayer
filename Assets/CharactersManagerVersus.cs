using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManagerVersus : CharactersManager {

	public List<CharacterBehavior> charactersTeam1;
	public List<CharacterBehavior> charactersTeam2;
	public Transform team1Container;
	public Transform team2Container;
	public float totalDistance;

	public override void Init()
	{
		totalDistance = ( Data.Instance.versusManager.area.z_length) - 2;
		distance = (totalDistance/2) * -1;
		//Data.Instance.events.OnAlignAllCharacters += OnAlignAllCharacters;
		//Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
		//Data.Instance.events.OnReorderAvatarsByPosition += OnReorderAvatarsByPosition;
		Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
		Data.Instance.events.OnAvatarFall += OnAvatarFall;
		//Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
		//Data.Instance.events.OnAutomataCharacterDie += OnAutomataCharacterDie;
		Data.Instance.events.OnAvatarDie += OnAvatarDie;

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
	void OnDestroy()
	{
		Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
		Data.Instance.events.OnAvatarFall -= OnAvatarFall;
		Data.Instance.events.OnAvatarDie -= OnAvatarDie;
	}
	void OnAvatarDie(CharacterBehavior cb)
	{
		if (cb.team_for_versus == 1)
			charactersTeam1.Remove (cb);
		else if (cb.team_for_versus == 2)
			charactersTeam2.Remove (cb);

		if (charactersTeam1.Count == 0 && charactersTeam2.Count == 0)
			Finish ();
	}
	public override Vector3 getPositionByTeam(int teamId)
	{
		List<CharacterBehavior> charactersByTean;

		if(teamId == 1)
			charactersByTean = charactersTeam1;
		else
			charactersByTean = charactersTeam2;
		
		if (charactersByTean.Count > 1)
		{
			Vector3 normalPosition = Vector3.zero;
			Vector3 lastCharacterPosition = Vector3.zero;
			float MaxDistance = 0;
			
			foreach(CharacterBehavior cb in charactersByTean)
			{
				if(lastCharacterPosition != Vector3.zero)
				{
					float dist = Vector3.Distance(cb.transform.localPosition, lastCharacterPosition);
					if(dist>MaxDistance) MaxDistance = dist;
				}
				lastCharacterPosition = cb.transform.localPosition;
				normalPosition += lastCharacterPosition;
			}

			normalPosition /= charactersByTean.Count;
			normalPosition.y += 0.15f + (MaxDistance / 4f );
			normalPosition.z -= 0.2f + (MaxDistance/26);

			return normalPosition;
		}
		else if (charactersByTean.Count == 0) return characterPosition;
		else
			characterPosition = charactersByTean[0].transform.localPosition;

		return characterPosition;
	}
	public override void OnUpdate()
	{
		print (distance + " totalDistance" + totalDistance);
		if (distance > totalDistance - 24) {			
			Finish ();
		}
	}
	void Finish()
	{
		Data.Instance.events.OnGameOver ();
		return;
	}
}
