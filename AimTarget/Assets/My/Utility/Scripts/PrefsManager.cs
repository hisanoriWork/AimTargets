using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace My {
  /*[Serializable]*/
  public class UserPrefs {
    public bool dataExists;
    public int[] score = new int[6];
    public float[] rate = new float[6];
    public int[] score1 = new int[6];
    public float[] rate1 = new float[6];
    public int[] score2 = new int[6];
    public float[] rate2 = new float[6];
  }
  public class PrefsManager {
    UserPrefs mUserPrefs = new UserPrefs();
    public UserPrefs GetUserPrefs() {
      string json = PlayerPrefs.GetString("userprefs", "NoData");
      if (json == "NoData")
        return mUserPrefs;
      else {
        mUserPrefs = JsonUtility.FromJson<UserPrefs>(json);
        return mUserPrefs;
      }
    }
    public bool SetUserPrefs(UserPrefs userprefs) {
      mUserPrefs = userprefs;
      string json = JsonUtility.ToJson(mUserPrefs);
      PlayerPrefs.SetString("userprefs", json);
      return true;
    }
    public void Delete() { PlayerPrefs.DeleteAll(); }
  }
}