using UnityEngine;
using System.Collections;

public class SceneObject : MonoBehaviour {

    public Level.Dificult Dificult;

    //sirve para quitar el objeto de pantalla màs tarde...
    public int size_z = 0;

	public bool broken;
    public int id;

    [HideInInspector]
    public Transform characterTransform;

    [HideInInspector]
    public bool isActive;
    public int score;

    [HideInInspector]
    public CharactersManager charactersMmanager;

    public int distanceFromCharacter;

    private Transform[] childs;

    //se dibuja solo si hay mas de un avatar vivo:
    public bool onlyMultiplayers;

    void Start()
    {
        
    }
    public void LateUpdate()
    {
        if (!isActive) return;
        if (!charactersMmanager) return;
        if (charactersMmanager.getDistance() == 0) return;
        if (!characterTransform) characterTransform = charactersMmanager.character.transform;
       
        float distance = charactersMmanager.getDistance();
        distanceFromCharacter = (int)transform.position.z - (int)distance;

        if (transform.localPosition.y < -6)
            Pool();
		else if (distance > transform.position.z + size_z + 22 && Data.Instance.playMode != Data.PlayModes.VERSUS)
            Pool();
		else if (distance > transform.position.z - 45 || Data.Instance.playMode == Data.PlayModes.VERSUS)
            OnSceneObjectUpdate();
    }
    public void Restart(Vector3 pos)
    {
        if (onlyMultiplayers && Game.Instance.level.charactersManager.getTotalCharacters() <= 1) 
            Pool();
        else
            OnRestart(pos);
    }
    public void setRotation(Vector3 rot)
    {
        if (transform.localEulerAngles == rot) return;
        transform.localEulerAngles = rot;
    }
    public void lookAtCharacter()
    {
       // transform.LookAt(characterTransform);
    }
    public void Pool()
    {
        isActive = false;
        Vector3 newPos = new Vector3(2000, 0, 2000);
        transform.position = newPos;       
        ObjectPool.instance.PoolObject(this);
        OnPool();
    }
    public virtual void OnSceneObjectUpdate()
    {
        SendMessage("OnSceneObjectUpdated", SendMessageOptions.DontRequireReceiver);
    }
    public virtual void OnRestart(Vector3 pos)
    {
        if (!Game.Instance)
        {
            Pool(); return;
        }
        if(!charactersMmanager)
         charactersMmanager = Game.Instance.GetComponent<CharactersManager>();

        gameObject.SetActive(true);
        transform.position = pos;
        isActive = true;
    }
    public virtual void changeMaterial(string materialName)
    {

    }
    public virtual void OnPool()
    {
    }
    public virtual void onDie()
    {

    }
    public virtual void setScore()
    {
    }    

	Color matColor;
	int videoGameID = -1;
	public void SetMaterialByVideoGame()
	{
		matColor = Data.Instance.videogamesData.GetActualVideogameData ().floor_top;
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		int newVideoGameID = Data.Instance.videogamesData.actualID;
		if (newVideoGameID != videoGameID) {
			videoGameID = newVideoGameID;
			foreach(Renderer r in renderers)
				ChangeMaterials(r);
		}
	}
	void ChangeMaterials(Renderer renderer)
	{
		renderer.material.color = matColor;
	}
}
