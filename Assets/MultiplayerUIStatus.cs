using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerUIStatus : MonoBehaviour {

    public int id;
    public Text field1;
    public Text field2;
    public Image background;
    public int score;
    public Animation anim;
    public GameObject deadMask;
    private float timeDead = 3;
    private bool done;

    public states state;
    public enum states
    {
        METE_FICHA,
        PLAYING,
        DEAD,
        WAITING_TO_RESTART
    }

	public void Init(int id, Color color, bool active) {
        this.id = id;
        background.color = color;
      //  field1.color = color;
        AddScore(0);
        if (!active) Inactive();
	}
    public void AddScore(int _score)
    {
        score += _score;

        if (state == states.DEAD) return;

        field1.text = score.ToString();
        field2.text = score.ToString();
        
        anim.Play("onScore");
    }
    public void Inactive()
    {
        state = states.METE_FICHA;
        field1.text = "METÉ FICHA";
        field2.text = "METÉ FICHA";
        anim.Play("inactive");
    }
    public void Dead()
    {
        state = states.DEAD;
        Vector3 pos = deadMask.transform.localPosition;
        pos.x = 0;
        deadMask.transform.localPosition = pos;

        anim.Play("dead");

        //Hashtable tweenData = new Hashtable();
        //tweenData.Add("x", -100);
        //tweenData.Add("time", timeDead);
        //tweenData.Add("easeType", iTween.EaseType.linear);
        //tweenData.Add("onCompleteTarget", this.gameObject);
        //tweenData.Add("onComplete", "DeadReady");
        //iTween.MoveTo(deadMask.gameObject, tweenData);
    }
    void Update()
    {
       
        if (state == states.DEAD)
        {
             Vector2 pos = deadMask.gameObject.transform.localPosition;
             if (pos.x > -100)
             {
                 pos.x -= Time.deltaTime * 30;
                 deadMask.gameObject.transform.localPosition = pos;
             }
             else
             {
                 DeadReady();
             }
        }
    }
    public void MoveTo(int _y)
    {
        Hashtable tweenData = new Hashtable();
        tweenData.Add("y", _y);
        tweenData.Add("time", 0.7f);
        tweenData.Add("islocal", true);
        tweenData.Add("easeType", iTween.EaseType.easeInOutQuad);
        iTween.MoveTo(gameObject, tweenData);
    }
    void DeadReady()
    {
        state = states.WAITING_TO_RESTART;
        Inactive();
    }
    public void Active()
    {
        AddScore(0);
    }
    public void Reset()
    {
       // Destroy(this.gameObject);
    }
}
