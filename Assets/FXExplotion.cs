﻿using UnityEngine;
using System.Collections;

public class FXExplotion : SceneObject {

	public MeshRenderer meshRenderer;
	float finalScale = 7;
	float speed = 5;	

	bool isOn;
	float timer;
	bool getBigger;

    public override void OnRestart(Vector3 position)
    {
		getBigger = true;
		isOn = true;
        position.y += 3;

		transform.localScale = Vector3.one;

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
	public void SetColor(Color color)
	{
		if (lastColor == color)
			return;		
		lastColor = color;
		color.a = 0.075f;
		meshRenderer.material.color = color;
	}
	public void OnSceneObjectUpdated()
	{
		if (!isOn)
			return;

		if (transform.localScale.x <= 0.2f && !getBigger)
			Pool ();
	//	timer += Time.deltaTime;

	//	Vector3 scale = transform.localScale;
		//float s = (timer * finalScale) / _duration;

		if(getBigger)
			transform.localScale = Vector3.Lerp(transform.localScale , new Vector3(finalScale,finalScale,finalScale), Time.deltaTime*speed);
		else
			transform.localScale = Vector3.Lerp(transform.localScale , Vector3.zero, Time.deltaTime*(speed*2));

		if (transform.localScale.x >= (finalScale - 0.5f))
			getBigger = false;
	}
    private void die()
    {
		isOn = false;
        Pool();
	}
	
}