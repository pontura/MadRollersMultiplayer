using UnityEngine;
using System.Collections;

public class FollowCharacter : MmoCharacter {

	float activaTionDistance = 8;
	float speed = 3;
	CharacterBehavior characterBehavior;
	float realPositionZ = 0;

	public override void OnRestart(Vector3 pos)
	{
		transform.localEulerAngles = Vector3.zero;
		realPositionZ = 0;
		base.OnRestart(pos);
		Repositionate ();
	}
	void Repositionate()
	{
		CharacterBehavior ch = charactersMmanager.getMainCharacter ();
		Vector3 myPos = transform.position;
		myPos.z = ch.transform.position.z - activaTionDistance;
		transform.position = myPos;
	}
	public void OnSceneObjectUpdated()
    {		
		CharacterBehavior cb = charactersMmanager.getMainCharacter ();
		if (cb == null)
			return;
		
		realPositionZ +=  (speed * Time.deltaTime);

        Vector3 pos = transform.position;
		pos.z = Mathf.Lerp(transform.position.z,  cb.transform.position.z - activaTionDistance + realPositionZ, 0.1f);
		transform.position = pos;

		if (transform.position.z > cb.transform.position.z + 40)
			Pool ();
    }
}

