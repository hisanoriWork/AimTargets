using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UnityEngine.UI;

namespace My {
  public class GameManager : MonoBehaviour {
    /*****public field*****/
    /*UI*/
    public Text scoreUI;
    public Text rateUI;
    public GameObject startScreen;
    public GameObject resultScreen;
    public GameObject settingScreen;
    public Canvas gameCanvas;
    public ResultController resultController;
    public GameObject pointObj;
    public Transform cameraT;
    public Vector3 displayPointPos;
    /*Game*/
    public FPSPlayer player;
    public Curve oddCurve;
    public float rate {
      get { return mRate; }
      set {
        mRate = value;
        rateUI.text = ((int)mRate).ToString() + "%";
      }
    }
    public int shotCount {
      get { return mShotCount; }
      set {
        mShotCount = value;
        rate = shotCount > 0 ? (float)hitCount / (float)shotCount * 100f : 100f;
        rateUI.text = ((int)rate).ToString() + "%";
      }
    }
    public int hitCount {
      get { return mHitCount; }
      set {
        mHitCount = value;
        rate = shotCount > 0 ? (float)hitCount / (float)shotCount * 100f : 100f;
        rateUI.text = ((int)rate).ToString() + "%";
      }
    }
    public int score {
      get { return mScore; }
      set { mScore = value; scoreUI.text = score.ToString(); }
    }
    /*****private field*****/
    /*Game*/
    int mScore;
    float mRate = 100f;
    int mShotCount;
    int mHitCount;
    int mShotToShot = 0;
    TargetManager mTargetManager;
    Timer mTimer;
    /*****monobehaviour method*****/
    void Awake() {
      mTargetManager = GetComponent<TargetManager>();
      mTimer = GetComponent<Timer>();
      //startScreen.SetActive(true);
      player.SetMouseCursorVisible(false);
      mTimer.Stop();
      Reset();
    }
    void Start() {
      BGMManager.instance.Play("Mercury");
    }
    void Update() {
      if (Input.GetKey(KeyCode.Escape)) Quit();
    }
    /*****public method*****/
    public void StartGame() {
      Reset();
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
        settingScreen.SetActive(true);
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
        mShotToShot++;
      });
      disp2 = mTargetManager.onTargetBreak.Subscribe(_ =>{
        hitCount++;
        if (mShotToShot == 0) oddCurve.AddTime(1);
        else oddCurve.AddTime(-mShotToShot*2);
        int point = (int)(oddCurve.GetValue() * 100);
        score += point;
        mShotToShot = -1;
        GameObject obj = Instantiate(pointObj, player.raycastHitPos, Quaternion.identity);
        DisplayPoint displayPoint = obj.GetComponent<DisplayPoint>();
        if(displayPoint){
          displayPoint.TextUpdate(point);
          obj.transform.rotation = Quaternion.LookRotation(cameraT.forward);
          obj.transform.position += displayPointPos;
        }
      });
      mTimer.whenTimeIsUp.Subscribe(time =>{
        disp1.Dispose();
        disp2.Dispose();
      });
    }
    void Reset() {
      mShotToShot = 0;
      score = 0;
      rate = 100;
      hitCount = 0;
      shotCount = 0;
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