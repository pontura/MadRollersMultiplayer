using UnityEngine;
using System.Collections;

public class FXExplotion : SceneObject {

	float finalScale = 3;
	float _duration = 0.2f;	

	bool isOn;
	float timer;

    public override void OnRestart(Vector3 position)
    {
		isOn = true;
        position.y += 3;

		transform.localScale = Vector3.zero;

        GameCamera camera = Game.Instance.gameCamera;

        float distance = transform.position.z - Game.Instance.GetComponent<CharactersManager>().getPosition().z;
        distance /= 2;

        float explotionPower = 5 - distance;

        if (explotionPower < 1.5f) explotionPower = 1.5f;
        else if (explotionPower > 3.5f) explotionPower = 3.5f;

        camera.explote(explotionPower);

        base.OnRestart(position);

        setScore();

        position.z += 0;
        position.y += 2;
		timer = 0f;
	}
	public void OnSceneObjectUpdated()
	{
		if (!isOn)
			return;
		
		timer += Time.deltaTime;

		Vector3 scale = transform.localScale;
		float s = (timer * finalScale) / _duration;

		transform.localScale = new Vector3(s,s,s);

		if (scale.x > finalScale)
			Pool ();
	}
    private void die()
    {
		isOn = false;
        Pool();
	}
	
}