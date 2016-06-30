using UnityEngine;
using System.Collections;
using System.Linq;

public class SceneObjectsBehavior : MonoBehaviour {

	public ArrayList unused = new ArrayList();
    [HideInInspector]
    public Area area;

    public SceneObject FloorSlider;
    public SceneObject FloorSurface;
    public SceneObject house1;
    public SceneObject house2;
    public SceneObject house3;
    public SceneObject house4;
    public SceneObject PisoPinche;
    public SceneObject rampa;
    public SceneObject rampaHuge;
    public SceneObject flyer;
	public SceneObject bomb1;
    public SceneObject palm;
    public SceneObject palm2;
    public SceneObject palm3;
    public SceneObject palm4;
    public SceneObject palm5;

    public SceneObject GrabbableJetpack;
    public SceneObject borde1;
   // public SceneObject enemyFrontal1;
    
    public SceneObject fences;
    public SceneObject rainbow;
    public SceneObject Listener;
    
    public SceneObject tunel1;
    public SceneObject tunel2;
    public SceneObject jumper;

    public SceneObject cruz;
    public SceneObject CruzGrande;
    public SceneObject rueda1;
    public SceneObject helice1;
    public SceneObject helice2;
    public SceneObject levelSignal;
    public SceneObject streetFloor;
    public SceneObject streetFloorSmall;
    public SceneObject subibaja;
    public SceneObject cepillo;
    public SceneObject pisoRotatorio;
    public SceneObject wallBig;
    public SceneObject wallMedium;
    public SceneObject wallSmall;
    public SceneObject wallSuperSmall;
    public SceneObject sombrilla;
    public SceneObject GrabbableMissile;

	private Game game;
    private ObjectPool Pool;

    private void Start()
    {
        game = Game.Instance;
        Pool = Data.Instance.sceneObjectsPool;
    }
	public void Add(GameObject go)
	{
		go.transform.parent = transform;
		unused.Add(go);
	}
	public GameObject GetUnusedObject(string name)
	{
		foreach (GameObject go in unused)
		{
			if(go && go.name == name + "_real(Clone)")
			{
				unused.Remove(go);
				return go;
			}
		}
		return null;
	}
	private void resetGO(GameObject go) {
		go.GetComponentInChildren<Renderer>().enabled = true;
	}
	public void replaceSceneObject(Area area, float areasLength, int areasX)
	{
        this.area = area;
        GameObject[] gos = area.getSceneObjects();
        bool nubesOn = false;
        foreach (GameObject go in gos)
        {
            SceneObject sceneObject = null;
            Vector3 pos = go.transform.position;
            pos.z += areasLength;
            pos.x += areasX;

            if (go.GetComponent<RandomPosition>())
            {
                RandomPosition mo = go.GetComponent<RandomPosition>();
                pos = mo.getPosition(pos);
            }

            if (!nubesOn)
            {
                nubesOn = true;
                addDecoration("Nubes_real", pos, new Vector3(0, Random.Range(0,2), 5));

            }

            switch (go.name)
            {
                case "extralargeBlock1":
                case "largeBlock1":
                case "mediumBlock1":
                case "smallBlock1":
                case "extraSmallBlock1":
                case "Coin":
                case "bloodx1":
                case "Yuyo":
                case "enemyFrontal":
                    
                    sceneObject = Pool.GetObjectForType(go.name + "_real", false);

                   

                    //HACK creo que esto arregla que desaparezca cada tanto un objeto, sino es asi borrame!
                    sceneObject.isActive = false;

                    if (sceneObject)
                    {
                        sceneObject.Restart(pos);
                        sceneObject.transform.rotation = go.transform.rotation;


                        if (go.GetComponent<MaterialsChanger>())
                        {
                            MaterialsChanger mo = go.GetComponent<MaterialsChanger>();
                            sceneObject.changeMaterial(mo.materialName);
                        }
                        else
                        {
                            sceneObject.changeMaterial("pasto");
                            if (go.name == "extralargeBlock1")
                            {
                                int num = Random.Range(1, 4);
                                string decorationName = "";
                                if (num == 1)
                                    decorationName = "flores1_real";
                                if (num == 2)
                                    decorationName = "flores2_real";
                                else if (num == 3)
                                    decorationName = "floorFlowers_real";

                                if (decorationName != "")
                                    addDecoration(decorationName, pos, Vector3.zero);

                            }
                        }
                        if (go.GetComponent<DecorationManager>())
                        {
                            addDecoration("Baranda1_real", pos, new Vector3(5.5f, 0, 3));
                            addDecoration("Baranda1_real", pos, new Vector3(-5.5f, 0, 3));
                            addDecoration("Baranda1_real", pos, new Vector3(5.5f, 0, -3));
                            addDecoration("Baranda1_real", pos, new Vector3(-5.5f, 0, -3));
                        }

                        
                    }
                    else
                    {
                        Debug.LogError("___________NO EXISTIO EL OBJETO: " + go.name);
                        Time.timeScale = 0;
                    }
                    break;
            }
            
            


                SceneObject clone = null;


                if (go.name == "FloorSurface")
                    clone = FloorSurface;
                if (go.name == "PisoPinche")
                    clone = PisoPinche;
                else if (go.name == "house1")
                    clone = house1;
                else if (go.name == "house2")
                    clone = house2;
                else if (go.name == "house3")
                    clone = house3;
                else if (go.name == "house4")
                    clone = house4;
                else if (go.name == "rampa")
                    clone = rampa;
                else if (go.name == "rampaHuge")
                    clone = rampaHuge;
                else if (go.name == "flyer")
                    clone = flyer;
                else if (go.name == "wallBig")
                {
                  //  addDecorationWithRotation("Graffiti_Real", pos, go.transform.localEulerAngles);
                    clone = wallBig;
                }
                else if (go.name == "wallMedium")
                    clone = wallMedium;
                else if (go.name == "wallSmall")
                    clone = wallSmall;
                else if (go.name == "wallSuperSmall")
                    clone = wallSuperSmall;
                else if (go.name == "jumper")
                    clone = jumper;
                else if (go.name == "bomb1")
                    clone = bomb1;
                else if (go.name == "tunel1")
                    clone = tunel1;
                else if (go.name == "tunel2")
                    clone = tunel2;
                else if (go.name == "palm")
                {
                    int ran = Random.Range(0, 100);
                    if (ran < 20)
                        clone = palm;
                    else if (ran < 40)
                        clone = palm2;
                    else if (ran < 60)
                        clone = palm3;
                    else if (ran < 80)
                        clone = palm4;
                    else
                        clone = palm5;

                    //Vector3 pos2 = go.transform.localEulerAngles;
                    //pos2.y = Random.Range(0, 360);
                    //go.transform.localEulerAngles = pos2;   
                }
                else if (go.name == "streetFloor")
                    clone = streetFloor;
                else if (go.name == "streetFloorSmall")
                    clone = streetFloorSmall;
                else if (go.name == "levelSignal")
                    clone = levelSignal;
                else if (go.name == "GrabbableJetpack")
                    clone = GrabbableJetpack;
                else if (go.name == "borde1")
                    clone = borde1;
                else if (go.name == "fences")
                    clone = fences;
                else if (go.name == "enemyFrontal")
                {
                    // int random = Random.Range(1, 4);
                    //if (random < 2)
                   // clone = enemyFrontal1;
                    //else if (random < 3)
                    //    clone = enemyFrontal2;
                    //else
                    //    clone = enemyFrontal3;
                }
                else if (go.name == "rainbow")
                    clone = rainbow;
                else if (go.name == "Listener")
                {
                    clone = Listener;
                }
                else if (go.name == "cruz")
                    clone = cruz;
                else if (go.name == "CruzGrande")
                    clone = CruzGrande;
                else if (go.name == "rueda1")
                    clone = rueda1;
                else if (go.name == "helice1")
                    clone = helice1;
                else if (go.name == "helice2")
                    clone = helice2;
                else if (go.name == "subibaja")
                    clone = subibaja;
                else if (go.name == "cepillo")
                    clone = cepillo;
                else if (go.name == "pisoRotatorio")
                    clone = pisoRotatorio;
                else if (go.name == "sombrilla")
                    clone = sombrilla;
                else if (go.name == "GrabbableMissile")
                    clone = GrabbableMissile;
                else if (go.name == "FloorSlider")
                    clone = FloorSlider;


                if (clone)
                {
                    sceneObject = Instantiate(clone, pos, Quaternion.identity) as SceneObject;
                    sceneObject.transform.parent = Pool.Scene.transform;
                    sceneObject.transform.rotation = go.transform.rotation;
                    sceneObject.Restart(pos);
                }
                if (go.GetComponent<MoveObject>())
                {
                    MoveObject mo = go.GetComponent<MoveObject>();
                    CopyComponent(mo, sceneObject.gameObject);
                }
                if (go.GetComponent<Dropper>())
                {
                    Dropper mo = go.GetComponent<Dropper>();
                    CopyComponent(mo, sceneObject.gameObject);
                }

                if (go.GetComponent<EnemyPathRunnerBehavior>())
                {
                    EnemyPathRunnerBehavior mo = go.GetComponent<EnemyPathRunnerBehavior>();
                    CopyComponent(mo, sceneObject.gameObject);
                }
                if (go.GetComponent<EnemyShooter>())
                {
                    EnemyShooter mo = go.GetComponent<EnemyShooter>();
                    CopyComponent(mo, sceneObject.gameObject);
                }
                if (go.GetComponent<EnemyRunnerBehavior>())
                {
                    EnemyRunnerBehavior mo = go.GetComponent<EnemyRunnerBehavior>();
                    CopyComponent(mo, sceneObject.gameObject);
                }
                if (go.GetComponent<Jump>())
                {
                    Jump mo = go.GetComponent<Jump>();
                    CopyComponent(mo, sceneObject.gameObject);
                }


                if (go.GetComponent<Subibaja>())
                {
                    Subibaja mo = go.GetComponent<Subibaja>();
                    CopyComponent(mo, sceneObject.gameObject);
                }

                if (go.GetComponent<ListenerDispatcher>())
                {
                    ListenerDispatcher mo = go.GetComponent<ListenerDispatcher>();
                    CopyComponent(mo, sceneObject.gameObject);
                }
                if (go.GetComponent<FollowCharacter>())
                {
                    FollowCharacter mo = go.GetComponent<FollowCharacter>();
                    CopyComponent(mo, sceneObject.gameObject);
                }
                if (go.GetComponent<FlyingBehavior>())
                {
                    FlyingBehavior mo = go.GetComponent<FlyingBehavior>();
                    CopyComponent(mo, sceneObject.gameObject);
                }
                if (go.GetComponent<FullRotation>())
                {
                    FullRotation mo = go.GetComponent<FullRotation>();
                    CopyComponent(mo, sceneObject.gameObject);
                }
                if (go.GetComponent<Bumper>())
                {
                    Bumper mo = go.GetComponent<Bumper>();
                    CopyComponent(mo, sceneObject.gameObject);
                }
			
	    }
	}
    public void PoolSceneObjectsInScene()
    {
        GameObject SceneObjectsContainer = Data.Instance.sceneObjectsPool.Scene;
        SceneObject[] sceneObjects = SceneObjectsContainer.GetComponentsInChildren<SceneObject>();
        foreach (SceneObject sceneObject in sceneObjects)
        {
            if(sceneObject.isActive)
                 sceneObject.Pool();
        }
        //Debug.Log("PoolSceneObjectsInScene qty = " + sceneObjects.Length);
    }
    Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
    public void addDecoration(string name, Vector3 pos, Vector3 offset)
    {
        SceneObject newSceneObject = Pool.GetObjectForType(name, false);
        pos.z += offset.z;
        pos.x += offset.x;
        newSceneObject.Restart(pos);
    }
    public void addDecorationWithRotation(string name, Vector3 pos, Vector3 rotation)
    {
        SceneObject newSceneObject = Pool.GetObjectForType(name, false);
        newSceneObject.Restart(pos);
        newSceneObject.transform.localEulerAngles = rotation;
    }
	
	public void deleteAll()
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag("sceneObject");
		 foreach (var go in objects)
		{
			Destroy(go);
		}
	}	

}
