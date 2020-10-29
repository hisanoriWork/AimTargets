using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UnityEngine.UI;

namespace My {
  public class GameManager : MonoBehaviour {
    /*****public field*****/
    public Text scoreUI;
    public Text rateUI;
    public FPSPlayer player;
    public float rate = 100;
    public int shotCount = 0;
    public int hitCount = 0;
    public int score = 0;
    public GameObject startScreen;
    public GameObject resultScreen;
    public Canvas gameCanvas;
    public ResultController resultController;
    /*****private field*****/
    TargetManager mTargetManager;
    Timer mTimer;
    /*****monobehaviour method*****/
    void Awake() {
      mTargetManager = GetComponent<TargetManager>();
      mTimer = GetComponent<Timer>();
      //startScreen.SetActive(true);
      player.SetMouseCursorVisible(false);
      mTimer.Stop();
    }
    void Update() {
      if (Input.GetKey(KeyCode.Escape)) Quit();
    }
    /*****public method*****/
    public void StartGame() {
      mTimer.Play();
      TriggerGameSet();
      TriggerGameDataRegister();
      mTargetManager.CreateTarget();
      mTargetManager.CreateTarget();
      mTargetManager.CreateTarget();
    }
    /*****private method*****/
    void TriggerGameSet() {
      mTimer.whenTimeIsUp.Take(1).Subscribe(time => {
        resultScreen.SetActive(true);
        mTargetManager.ClearTarget();
        resultController.Register(score, rate);
        Reset();
      });
    }
    void TriggerGameDataRegister() {
      IDisposable disp1;
      IDisposable disp2;
      disp1 = player.onShot.Subscribe(_ =>{
        shotCount++;
        rate = shotCount > 0 ? (float)hitCount / (float)shotCount * 100f : 100f;
        rateUI.text = ((int)rate).ToString() + "%";
      });
      disp2 = mTargetManager.onTargetBreak.Subscribe(_ =>{
        hitCount++;
        score += 100;
        scoreUI.text = score.ToString();
      });
      mTimer.whenTimeIsUp.Subscribe(time =>{
        disp1.Dispose();
        disp2.Dispose();
      });
    }
    void Reset() {
      score = 0;
      rate = 100;
      hitCount = 0;
      shotCount = -1;
      scoreUI.text = score.ToString();
      rateUI.text = ((int)rate).ToString() + "%";
    }
    void Quit() {
      #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
      #elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
      #endif
    }
  }

}