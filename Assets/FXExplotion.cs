using UnityEngine;
using System.Collections;

public class FXExplotion : SceneObject {

    public float _scale = 3;
	public float _duration = 0.5f;	
	private bool isScaling = false;
	private int floorDumps = 0;
	private int floorTotalDumps = 0;


    public override void OnRestart(Vector3 position)
    {
        position.y += 3;

        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        iTween.ScaleTo(
            gameObject, 
            iTween.Hash(
                "scale", Vector3.one * _scale,
                "time", _duration,
                "onComplete", "die",
                "easeType", iTween.EaseType.linear
            )
        );

        GameCamera camera = Game.Instance.gameCamera;

        float distance = transform.position.z - Game.Instance.GetComponent<CharactersManager>().getPosition().z;
        distance /= 2;

        float explotionPower = 5 - distance;

        if (explotionPower < 1.5f) explotionPower = 1.5f;
        else if (explotionPower > 3.5f) explotionPower = 3.5f;

        camera.explote(explotionPower);

        base.OnRestart(position);

        setScore();

        position.z -= 2;
        position.y += 2;
	}

    private void die()
    {
        Pool();
	}
	
}