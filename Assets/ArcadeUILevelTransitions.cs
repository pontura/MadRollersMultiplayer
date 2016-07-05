using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArcadeUILevelTransitions : MonoBehaviour {

    public GameObject panel;
    public GameObject texts;

    private int level;

	void Start () {
        level = 1;
        panel.SetActive(false);
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
	}
    void OnDestroy()
    {
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;
    }
    void OnListenerDispatcher(string type)
    {
        if (type == "LevelFinish")
        {
            panel.SetActive(true);
          //  panel.GetComponent<Animation>().Play("levelTransition");
            StartCoroutine(DoFade());
            foreach (Text field in texts.GetComponentsInChildren<Text>())
            {
                field.text = "Nivel " + level;
            }
            level++;
        }
	}
    void SetOff()
    {
        panel.SetActive(false);
    }

    public IEnumerator DoFade()
    {
        float t = 1;
        while (t > 0)
        {
            yield return new WaitForEndOfFrame();
            t -= Time.deltaTime/1.5f;
            RenderSettings.ambientIntensity = t;
        }
        yield return new WaitForSeconds(0.1f);
        foreach (Text field in texts.GetComponentsInChildren<Text>())
        {
            field.text = "Yeah!";
        }
        while (t < 1)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime/1.5f;
            RenderSettings.ambientIntensity = t;
        }
        SetOff();
    }
}
