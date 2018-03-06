using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level : MonoBehaviour {

    public bool waitingToStart;
    public Dificult Dificulty;
    public enum Dificult
    {
        ALL,
        EASY,
        MEDIUM,
        HARD
    }
    public SceneObject explotion;
    public SceneObject wallExplotion;

    public SceneObject explotionEffect;
	public SceneObject explotionGift;

	public GameObject limitObject;
    public ScoreSignal scoreSignal;

    public ProgressBar missionBar;

	public GameObject missionDesc;

	private AreasManager areasManager;
	private FloorManager floorManager;
	
	private float lastDistanceToLoadLevel;

    /// para arcade
    //private float nextDistanceVictoryArea;
    //private int distanceVictoryArea = 550;

	public Area victoryAreaLastLevel;
    public Area victoryArea;
    //////////////////////

	static Area areaActive;
	public float areasLength = 0;
	private int nextPlatformSpace = 30;
	public SceneObjectsBehavior sceneObjects;
	Game game;

	private Area skyArea;
	private Missions missions;	
	private bool showStartArea;
	private Data data;
    private bool playing;
    private int areasX;
    public CharactersManager charactersManager;
    private PowerupsManager powerupsManager;
	public bool isLastArea;
   
    public void SetDificultyByScore(int score)
    {
        if (score < 40) Dificulty = Dificult.EASY;
        else if (score < 85) Dificulty = Dificult.MEDIUM;
        else Dificulty = Dificult.HARD;
    }
    private void Awake()
    {
        Dificulty = Dificult.EASY;
		waitingToStart = true;
    }
    public void Init()
	{
       // nextDistanceVictoryArea = distanceVictoryArea;
        areasX = 0;
        playing = true;
        areaActive = null;
        
		data = Data.Instance;
        game = Game.Instance;
        missions = data.GetComponent<Missions>(); 		
        charactersManager = game.GetComponent<CharactersManager>();
        floorManager = GetComponent<FloorManager>();
        powerupsManager = GetComponent<PowerupsManager>();
        floorManager.Init(charactersManager);

		missions.Init(data.missions.MissionActiveID, this);
        areasManager = missions.getAreasManager();
        areasManager.Init(1);
		SetNewVideogameSettings ();
        areasLength = 0;

		if (!Data.Instance.isArcadeMultiplayer && !waitingToStart) // nunevo && !waitingToStart)
            missions.StartNext();

        data.events.OnResetLevel += reset;
        data.events.OnSetFinalScore += OnSetFinalScore;
        data.events.OnAddExplotion += OnAddExplotion;
        data.events.OnAddWallExplotion += OnAddWallExplotion;
        data.events.OnAddObjectExplotion += OnAddObjectExplotion;
        data.events.OnAddHeartsByBreaking += OnAddHeartsByBreaking;
        data.events.OnAddTumba += OnAddTumba;
        data.events.StartMultiplayerRace += StartMultiplayerRace;
        data.events.SetVictoryArea += SetVictoryArea;

		Invoke ("Delayed", 0.5f);
    }
	void Delayed()
	{
		Data.Instance.voicesManager.PlayRandom (Data.Instance.voicesManager.welcome);
	}
    
    public void OnDestroy()
    {
        data.events.OnResetLevel -= reset;
        data.events.OnSetFinalScore -= OnSetFinalScore;
        data.events.OnAddExplotion -= OnAddExplotion;
        data.events.OnAddWallExplotion -= OnAddWallExplotion;
        data.events.OnAddObjectExplotion -= OnAddObjectExplotion;
        data.events.OnAddTumba -= OnAddTumba;
        data.events.StartMultiplayerRace -= StartMultiplayerRace;
        data.events.SetVictoryArea -= SetVictoryArea;
        data.events.OnAddHeartsByBreaking -= OnAddHeartsByBreaking;
    }
    void StartMultiplayerRace()
    {
        waitingToStart = false;
    }
	public void Complete()
	{
		Data.Instance.voicesManager.PlayRandom (Data.Instance.voicesManager.missionComplete);
		showStartArea = true;
		missions.Complete();
		missions.StartNext();
		areasManager = missions.getAreasManager();
		areasManager.Init(0);
		data.setMission(missions.MissionActiveID);   
		SetNewVideogameSettings ();
	}
	void SetNewVideogameSettings()
	{

		VideogameData videogameData = Data.Instance.videogamesData.GetActualVideogameData ();
		RenderSettings.skybox = videogameData.skybox;
		RenderSettings.fogColor = videogameData.fog;
	}
	private void  reset()
	{
        if (!playing) return;
        playing = false;
        sceneObjects.PoolSceneObjectsInScene();
        //Init();
	}
    public void OnAddObjectExplotion(Vector3 position, int type)
    {
        Data.Instance.events.OnSoundFX("FX break", -1);
        SceneObject explpotionEffect;
        switch (type)
        {
            case 1:
                explpotionEffect = ObjectPool.instance.GetObjectForType("ExplotionEffectBomb", true); break;
            case 2:
                explpotionEffect = ObjectPool.instance.GetObjectForType("ExplotionEffectEnemy", true); break;
            default:
                explpotionEffect  = ObjectPool.instance.GetObjectForType("ExplotionEffectSimpleObject", true); break;
        }
        if (explpotionEffect)
            explpotionEffect.Restart(position);
    }
    public void OnAddExplotion(Vector3 position, Color color)
    {
        OnAddExplotion(position, explotion.name, explotionEffect.name, explotionGift.name, 3, color);
    }
    public void OnAddExplotion(Vector3 position, int force, Color color)
    {
        OnAddExplotion(position, explotion.name, explotionEffect.name, explotionGift.name, force, color);
    }
    public void OnAddWallExplotion(Vector3 position, Color color)
    {
        OnAddExplotion(position, wallExplotion.name, "ExplotionEffectWall", explotionGift.name, 3, color);
    }
    public void OnAddExplotion(Vector3 position, string _name, string _explotionEffect, string _explotionGift, int force, Color color)
	{
        Data.Instance.events.OnSoundFX("FX explot00", -1);
        Vector3 newPos = position;
        newPos.y -= 4;

        SceneObject explotionNew = ObjectPool.instance.GetObjectForType(_name, true);
        if (explotionNew == null)
        {
           // Debug.LogError("No hay explosion");
            return;
        }
       
        explotionNew.GetComponent<FXExplotion>()._scale = force;

        if (explotionNew)
            explotionNew.Restart(newPos);

        if (_explotionEffect != "")
        {
            SceneObject explpotionEffect = ObjectPool.instance.GetObjectForType(_explotionEffect, true);
            if (explpotionEffect)
            {
                explpotionEffect.Restart(newPos);
                ParticlesSceneObject ps = explpotionEffect.GetComponent<ParticlesSceneObject>();
                if (ps != null)
                    ps.SetColor(color);
            }

        }

		if ((Data.Instance.playMode == Data.PlayModes.STORY && Data.Instance.missions.MissionActiveID<7) 
            || !powerupsManager.CanBeThrown() 
            || Random.Range(0, 100) > 50
            || charactersManager.getDistance()<300
            )
            AddHeartsByBreaking(position, 12, 450);
        else
            Data.Instance.events.OnAddPowerUp(position);
	}
	void OnAddHeartsByBreaking(Vector3 position, Material[] mat, Vector3[] pos)
	{
		int force = 400;
		position.y += 0.7f;
		int NumOfParticles = mat.Length;
		for (int a = 0; a < NumOfParticles; a++)
		{
			SceneObject newSO = ObjectPool.instance.GetObjectForType(explotionGift.name, true);
			if (newSO)
			{
				newSO.Restart(pos[a]);
				newSO.transform.localEulerAngles = new Vector3(0, a * (360 / NumOfParticles), 0);
				Vector3 direction = ((newSO.transform.forward * force) + (Vector3.up * (force*2)));
				newSO.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
				GrabbableItem gi = newSO.GetComponent<GrabbableItem> ();
				gi.SetMaterial (mat[a]);
			}
		}
	}
    void AddHeartsByBreaking(Vector3 position, int NumOfParticles, int force = 400)
    {
        position.y += 0.7f;
        if (NumOfParticles > 20) NumOfParticles = 20;
        for (int a = 0; a < NumOfParticles; a++)
        {
            SceneObject newSO = ObjectPool.instance.GetObjectForType(explotionGift.name, true);
            if (newSO)
            {
                newSO.Restart(position);
                newSO.transform.localEulerAngles = new Vector3(0, a * (360 / NumOfParticles), 0);
                Vector3 direction = ((newSO.transform.forward * force) + (Vector3.up * (force*3)));
                newSO.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
				GrabbableItem gi = newSO.GetComponent<GrabbableItem> ();
				gi.SetGroundMaterial ();
            }
        }
    }
	private void  createNextArea(Area area)
	{
        if (areaActive)
            areasLength += areaActive.z_length / 2;
		areaActive = area;
        areasLength += area.z_length / 2;
               
        sceneObjects.replaceSceneObject(area, areasLength - 4, areasX);
        areasX += area.nextAreaX; 	   
	}
    bool showVictory;
	void SetVictoryArea()
    {
        print("__________________victory!");
        showVictory = true;
    }
	int tutorialID;
	private void Update () {
	
		float dist = charactersManager.getDistance ();

		////TUTORIAL
		print (tutorialID + "   " + missions.MissionActiveID + "   " + charactersManager.getDistance ());
		if(missions.MissionActiveID == 0)
		{
			if (dist>92 && tutorialID < 1)
			{
				Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.tutorials [0].audioClip);
				tutorialID = 1;
			} else if(missions.MissionActiveID == 0 && dist>130 && tutorialID < 2)
			{
				Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.tutorials [1].audioClip);
				tutorialID = 2;
			} else if(missions.MissionActiveID == 0 && dist>240 && tutorialID < 3)
			{
				Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.tutorials [2].audioClip);
				tutorialID = 3;
			}
		}
		////////////////
		/// 
        if (areasLength==0)
       {
           createNextArea(areasManager.getStartingArea());
			NewMissionAreaStart ();
		} else if (dist > (areasLength - nextPlatformSpace)
		&&
			lastDistanceToLoadLevel != dist)
		{
			lastDistanceToLoadLevel = dist;

            Area newArea;
            if(showVictory == true)
            {
                newArea = victoryArea;
                showVictory = false;
            } else
          //  if (lastDistanceToLoadLevel > nextDistanceVictoryArea)
         //   {
           //     nextDistanceVictoryArea = lastDistanceToLoadLevel + distanceVictoryArea;
           //     newArea = victoryArea;
          //  } else
			if(showStartArea)
			{
				if (isLastArea) {
					areasManager.showRelaxAreaBeforeStarting = true;
					newArea = victoryAreaLastLevel;
				}
				else
					newArea = areasManager.getRandomArea(true);					
				showStartArea = false; 
			} else 
			{
				newArea = areasManager.getRandomArea(false);
			}	
			createNextArea(newArea);
            //print("new area " + newArea.name + " lastDistanceToLoadLevel: " + lastDistanceToLoadLevel);
		}
	}
	public void NewMissionAreaStart()
	{
		isLastArea = areasManager.GetActiveAreaSet ().isLastArea;
	}
    public void FallDown(int fallDownHeight)
    {
        GameCamera camera = game.gameCamera;
        camera.fallDown(fallDownHeight);
    }
    public void OnSetFinalScore(int playerID, Vector3 position, int score)
    {
        if (position == Vector3.zero) return;
        SceneObject newSO = Instantiate(scoreSignal, position, Quaternion.identity) as SceneObject;
        newSO.Restart(position);
        newSO.GetComponent<ScoreSignal>().SetScore(playerID, score);
        Data.Instance.events.OnScoreOn(playerID, position, score);
    }
    public void addSceneObjectToScene(SceneObject _so, Vector3 position)
    {
        SceneObject so = Instantiate(_so, position, Quaternion.identity) as SceneObject;
        so.Restart(so.transform.position);
    }


    private bool onLeft;
    public void OnAddTumba(Vector3 position, string username, string facebookID)
    {
        onLeft = !onLeft;

        Vector3 newPos = position;
        newPos.y += 0f;

        if (onLeft)
        {
            newPos.x = -8;
        }
        else
        {
            newPos.x = 8;
        }

        SceneObject obj = ObjectPool.instance.GetObjectForType("Tumba_real", true);
        if (obj)
        {
            obj.Restart(newPos);
            obj.GetComponent<TumbaAvatar>().SetPicture(facebookID);
            obj.GetComponent<TumbaAvatar>().SetField(username);
            if (onLeft)
                obj.transform.localEulerAngles = new Vector3(0, 0, 0);
            else
                obj.transform.localEulerAngles = new Vector3(0, 90, 0);
        }
    }
}
