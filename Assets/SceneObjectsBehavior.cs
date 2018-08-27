using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneObjectsBehavior : MonoBehaviour {

	SceneObjectsManager manager;
	public ArrayList unused = new ArrayList();

	public SceneObject Water;
	public SceneObject Lava;
	public SceneObject Boss1;
	public SceneObject Boss2;
	public SceneObject BossCreator;
	public SceneObject BossSpace1;
	public SceneObject BossCalecitas1;
	public SceneObject BossPacmans;
	public SceneObject BossGalaga;
	public SceneObject BossPacmansIntro;
	public SceneObject Starting;
	public SceneObject Calecita;
	public SceneObject FloorSlider;
	public SceneObject FloorSurface;
	public SceneObject house1;
	public SceneObject house2;
	public SceneObject house3;
	public SceneObject house4;
	public SceneObject PisoPinche;
	public SceneObject rampa;
	public SceneObject rampaHuge;
	public SceneObject bomb1;
	public SceneObject palm;
	public SceneObject palm2;
	public SceneObject palm3;
	public SceneObject palm4;
	public SceneObject palm_tall;
	public SceneObject enemyGhost;
	public SceneObject cilindro;

	public SceneObject GrabbableInvensible;
	public SceneObject GrabbableJetpack;
	public SceneObject borde1;

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

	public Game game;
	private ObjectPool Pool;

	private void Awake()
	{
		manager = GetComponent<SceneObjectsManager> ();
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

	List<Transform> borderTransforms;
	public void replaceSceneObject(Area area, float areasLength, int areasX, bool oposite = false)
	{
		//print (area.name + " AREA");
		borderTransforms = new List<Transform> ();
		GameObject[] gos = area.getSceneObjects();
		bool nubesOn = false;

		foreach (GameObject go in gos)
		{
			SceneObject sceneObject = null;
			Vector3 pos = go.transform.position;
			pos.z += areasLength;
			pos.x += areasX;
			if (oposite) {
				pos.z *= -1;
			}

		
			//  if (!nubesOn)
			//  {
			//  nubesOn = true;
			//   addDecoration("Nubes_real", pos, new Vector3(0, Random.Range(0,2), 5));

			//  }

			switch (go.name)
			{
			case "extralargeBlock1":
			case "flyer":
			case "largeBlock1":
			case "mediumBlock1":
			case "smallBlock1":
			case "extraSmallBlock1":
			case "Coin":
			case "bloodx1":
			//case "Yuyo":
			case "enemyFrontal":   
			case "enemyWater":   
			case "enemySide":  
			case "enemyBack":  
			case "castle":
			case "SideMountain":
			case "bonusEntrance":   
			case "Cascade": 
			case "firewall":        
			case "Baranda1":  
			//case "Tumba":  
			case "enemyNaveSimple":  
			case "BichoVuela":
			case "palm":
			case "palm2":
			case "palm3":
			case "palm4":
			case "palmTall":
				if(go.name == "smallBlock1" || go.name == "extraSmallBlock1")
					sceneObject = Pool.GetObjectForType(go.name + "_real", true);    
				else
					sceneObject = Pool.GetObjectForType(go.name + "_real", false);  

				if (sceneObject)
				{
					


					sceneObject.isActive = false;
					//sceneObject.Restart(pos);
					sceneObject.transform.position = pos;
					sceneObject.transform.rotation = go.transform.rotation;

					//if (go.name == "Yuyo")
					//	sceneObject.SetMaterialByVideoGame ();
					
					//sceneObject.changeMaterial("pasto");
//
//					if (go.name == "extralargeBlock1")
//					{
//						GameObject goNew = new GameObject ();
//						goNew.transform.position = pos;
//						goNew.transform.rotation = go.transform.rotation;
//						borderTransforms.Add (goNew.transform);
//
////						int num = Random.Range(1, 4);
////						string decorationName = "";
////						if (num == 1)
////							decorationName = "flores1_real";
////						if (num == 2)
////							decorationName = "flores2_real";
////						else if (num == 3)
////							decorationName = "floorFlowers_real";
////
////						if (decorationName != "")
////							addDecoration(decorationName, pos, Vector3.zero);
//
//					}
					//  }
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
					Data.Instance.events.ForceFrameRate (0);
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
			else if (go.name == "wallBig") {
				//  addDecorationWithRotation("Graffiti_Real", pos, go.transform.localEulerAngles);
				clone = wallBig;
			} else if (go.name == "wallMedium")
				clone = wallMedium;
			else if (go.name == "wallSmall")
				clone = wallSmall;
			else if (go.name == "wallSuperSmall")
				clone = wallSuperSmall;
			else if (go.name == "jumper")
				clone = jumper;
			else if (go.name == "Lava")
				clone = Lava;
			else if (go.name == "Water")
				clone = Water;
			else if (go.name == "Boss1")
				clone = Boss1;
			else if (go.name == "Boss2")
				clone = Boss2;
			else if (go.name == "BossCalecitas1")
				clone = BossCalecitas1;
			else if (go.name == "BossCreator") 
				clone = BossCreator;
			else if (go.name == "BossSpace1")
				clone = BossSpace1;
			else if (go.name == "BossPacmans")
				clone = BossPacmans;
			else if (go.name == "BossGalaga")
				clone = BossGalaga;
			else if (go.name == "BossPacmansIntro")
				clone = BossPacmansIntro;			
			else if (go.name == "Calecita")
				clone = Calecita;
			else if (go.name == "Starting")
				clone = Starting;
			else if (go.name == "bomb1")
				clone = bomb1;
			else if (go.name == "tunel1")
				clone = tunel1;
			else if (go.name == "tunel2")
				clone = tunel2;
			else if (go.name == "cilindro")
				clone = cilindro;
			else if (go.name == "enemyGhost")
				clone = enemyGhost;
//			else if (go.name == "palmTall") {
//				clone = palm_tall;
//				go.transform.localEulerAngles = new Vector3 (0, Random.Range (0, 4) * 90,  0);
//			}
//			else if (go.name == "palm")
//			{
//				int ran = Random.Range (0, 60);
//				if (ran < 20)
//					clone = palm;
//				else if (ran < 40)
//					clone = palm2;
//				else 
//					clone = palm3;
//				
//				go.transform.localEulerAngles = new Vector3 (0, Random.Range (0, 4) * 90,  0);
//				//}
//			}
			else if (go.name == "streetFloor")
				clone = streetFloor;
			else if (go.name == "streetFloorSmall")
				clone = streetFloorSmall;
			else if (go.name == "levelSignal")
				clone = levelSignal;
			else if (go.name == "GrabbableJetpack")
				clone = GrabbableJetpack;
			else if (go.name == "GrabbableInvensible")
				clone = GrabbableInvensible;
			else if (go.name == "borde1")
				clone = borde1;
			else if (go.name == "fences")
				clone = fences;
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

				sceneObject.transform.rotation = go.transform.rotation;

				if (go.GetComponent<BossSettings>())
				{
					BossSettings mo = go.GetComponent<BossSettings>();
					CopyComponent(mo, sceneObject.gameObject);
				}

			//	sceneObject.Restart(pos);
			}
			if (go.GetComponent<Move>() && sceneObject.GetComponent<Move>() == null)
			{
				Move mo = go.GetComponent<Move>();
				CopyComponent(mo, sceneObject.gameObject);
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
			if (go.GetComponent<EnemyPathsMultiples>())
			{
				EnemyPathsMultiples mo = go.GetComponent<EnemyPathsMultiples>();
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
			if (go.GetComponent<RandomPosition>())
			{
				RandomPosition mo = go.GetComponent<RandomPosition>();
				pos = mo.getPosition(pos);
			}

			if(sceneObject!=null)
				manager.AddSceneObject (sceneObject, pos);
		}

			//AddBorders ();
		//Debug.LogError (" borderTransforms.Count " + borderTransforms.Count);
	}

	Component CopyComponent(Component original, GameObject destination)
	{
		System.Type type = original.GetType();
		Component copy = destination.AddComponent(type);
		System.Reflection.FieldInfo[] fields = type.GetFields();
		foreach (System.Reflection.FieldInfo field in fields)
		{
			field.SetValue(copy, field.GetValue(original));
		}
		return copy;
	}

	public void addDecoration(string name, Vector3 pos, Vector3 offset)
	{
		SceneObject newSceneObject = Pool.GetObjectForType(name, true);
		if (newSceneObject == null)
			return;
		pos.z += offset.z;
		pos.x += offset.x;
		newSceneObject.Restart (pos);
		newSceneObject.SetMaterialByVideoGame ();
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
