using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My {
  public class ResultController : MonoBehaviour {
    public ScoreRate[] srList;
    public ScoreRate mysr;
    public int[] scoreList = new int[6];
    public float[] rateList = new float[6];
    public PrefsManager prefsManager = new PrefsManager();
    void Awake() {
      for (int i = 0; i < scoreList.Length; i++)
        scoreList[i] = 0;
      for (int i = 0; i < rateList.Length; i++)
        rateList[i] = 0;
    }
    public void Register(int s, float r,GameType type) {
      mysr.SetScore(s);
      mysr.SetRate(r);
      UserPrefs user = prefsManager.GetUserPrefs();
      if (type == GameType.E3thTarget) {
        scoreList = user.score;
        rateList = user.rate;
      } else if (type == GameType.EFlick) {
        scoreList = user.score1;
        rateList = user.rate1;
      } else if (type == GameType.ETracking) {
        scoreList = user.score2;
        rateList = user.rate2;
      }
      int index = int.MaxValue;
      for (int i = 0; i < scoreList.Length; i++) {
        if (scoreList[i] < mysr.s ||
        scoreList[i] == mysr.s && rateList[i] > mysr.r) {
          index = i;
          break;
        }
      }
      if (index < scoreList.Length) {
        for (int i = scoreList.Length - 1; i > index; i--) {
          scoreList[i] = scoreList[i - 1];
          rateList[i] = rateList[i - 1];
        }
        scoreList[index] = s;
        rateList[index] = r;
      }
      for (int i = 0; i < scoreList.Length; i++) {
        srList[i].SetScore(scoreList[i]);
        srList[i].SetRate(rateList[i]);
      }
      if (type == GameType.E3thTarget) {
        user.score = scoreList;
        user.rate = rateList;
      } else if (type == GameType.EFlick) {
        user.score1 = scoreList;
        user.rate1 = rateList; ;
      } else if (type == GameType.ETracking) {
        user.score2 = scoreList;
        user.rate2 = rateList;
      }
      prefsManager.SetUserPrefs(user);
    }

    public void SetResult(GameType type) {
      mysr.SetScore("---");
      mysr.SetRate("--- ");
      UserPrefs user = prefsManager.GetUserPrefs();
      if (type == GameType.E3thTarget) {
        scoreList = user.score;
        rateList = user.rate;
      } else if (type == GameType.EFlick) {
        scoreList = user.score1;
        rateList = user.rate1;
      } else if (type == GameType.ETracking) {
        scoreList = user.score2;
        rateList = user.rate2;
      }
      for (int i = 0; i < scoreList.Length; i++) {
        srList[i].SetScore(scoreList[i]);
        srList[i].SetRate(rateList[i]);
      }
    }
  }
}
