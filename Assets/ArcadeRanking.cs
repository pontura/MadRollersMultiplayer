using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

public class ArcadeRanking : MonoBehaviour {

    public int newHiscore;
    private int totalHiscores = 5;

    [Serializable]
    public class RankingData
    {
        public int score;
        public Texture2D texture;
    }
	public List<RankingData> all;

    public void OnAddHiscore(Texture2D texture,  int _hiscore)
    {
        RankingData data = new RankingData();
        data.score = _hiscore;
        data.texture = texture;
        all.Add(data);
        Reorder();
    }
    public bool CheckIfEnterHiscore(int score)
    {
        if (score>50 && all.Count < totalHiscores) return true;

        if (score > all[totalHiscores-1].score)
            return true;

        return false;
    }
    void Start () {
		Data.Instance.events.OnHiscore += OnHiscore;
	}
	void OnHiscore(Texture2D texture, int _hiscore)
	{
        RankingData data = new RankingData();
        data.score = _hiscore;
        data.texture = texture;
        all.Add(data);
        Reorder();
        if (all.Count > totalHiscores)
            all.Remove(all[all.Count - 1]);
    }
    void Reorder()
    {
        all = all.OrderBy(w => w.score).ToList();
        all.Reverse();
    }
}
