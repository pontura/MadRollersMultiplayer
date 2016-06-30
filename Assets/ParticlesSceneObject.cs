using UnityEngine;
using System.Collections;

public class ParticlesSceneObject : SceneObject {

    public ParticleSystem _particleSystem;
    public ParticleSystem explotion;
    public ParticleSystem[] explotions_to_colorize;

    public override void OnRestart(Vector3 pos)
    {
        base.OnRestart(pos);

        if (_particleSystem)
        {
            _particleSystem.Clear();
            _particleSystem.Play();
        }

        explotion.Clear();
        explotion.Play();

        if (GetComponent<AudioSource>())
        {
           // GetComponent<AudioSource>().Play();
        }
        //GameObject go = Instantiate(esplotionParticles, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //pos.y += 5;
        //pos.z -= 2;
        //go.transform.localPosition = pos;
        //go.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
    }
    public void SetColor(Color color)
    {        
        color.a = 0.45f;

        foreach (ParticleSystem ps in explotions_to_colorize)
            ps.startColor = color;
    }

}
