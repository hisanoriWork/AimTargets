using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UnityEngine.UI;

namespace My {
  public enum GameType {
    EFlick,
    E3thTarget,
    ETracking,
    ENone,
  }
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
    public GameType gameType = GameType.ENone;
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
    float mAimTime = 0;
    float onTime = 0;
    float offTime = 0;
    TargetManager mTargetManager;
    Timer mTimer;
    /*****monobehaviour method*****/
    void Awake() {
      mTargetManager = GetComponent<TargetManager>();
      mTimer = GetComponent<Timer>();
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
      switch (gameType) {
        case GameType.E3thTarget:
          SetGameSetTrigger();
          SetScoreTrigger();
          Set3thTargetGameTrigger();
          mTargetManager.Create3thTarget();
          mTargetManager.Create3thTarget();
          mTargetManager.Create3thTarget();
          break;
        case GameType.EFlick:
          SetGameSetTrigger();
          SetScoreTrigger();
          SetFlickGameTrigger();
          mTargetManager.CreateFlickTarget();
          break;
        case GameType.ETracking:
          SetGameSetTrigger();
          SetScoreTrigger2();
          SetTrackingGameTrigger();
          mTargetManager.CreateTrackingTarget();
          break;
      }
    }
    public void SetGameType_3thTarget() {
      gameType = GameType.E3thTarget;
      resultController.SetResult(gameType);
    }
    public void SetGameType_Flick() {
      gameType = GameType.EFlick;
      resultController.SetResult(gameType);
    }
    public void SetGameType_Tracking() {
      gameType = GameType.ETracking;
      resultController.SetResult(gameType);
    }
    /*****private method*****/
    void SetGameSetTrigger() {
      mTimer.whenTimeIsUp.Take(1).Subscribe(time => {
        resultScreen.SetActive(true);
        settingScreen.SetActive(true);
        mTargetManager.ClearTarget();
        resultController.Register(score, rate,gameType);
        Reset();
      });
    }
    void SetScoreTrigger() {
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
    void SetScoreTrigger2() {
      int sumPoint = 0;
      IDisposable disp1 = mTargetManager.onTargetDamege.Subscribe(_ => {
        oddCurve.AddTime(0.01f);
        int point = (int)(oddCurve.GetValue()*1.5);
        score += point;
        sumPoint += point;
        shotCount++;
        hitCount++;
      });

      IDisposable disp2 = mTargetManager.onTargetDestroy.Subscribe(_ =>{
        GameObject obj = Instantiate(pointObj, player.raycastHitPos, Quaternion.identity);
        DisplayPoint displayPoint = obj.GetComponent<DisplayPoint>();
        if (displayPoint) {
          displayPoint.TextUpdate(sumPoint);
          obj.transform.rotation = Quaternion.LookRotation(cameraT.forward);
          obj.transform.position += displayPointPos;
          sumPoint = 0;
        }
      });

      IDisposable disp3 = player.onOff.Subscribe(_ => {
        oddCurve.AddTime(-1.0f);
        shotCount++;
      });
      mTimer.whenTimeIsUp.Subscribe(time => {
        disp1.Dispose();
        disp2.Dispose();
        disp3.Dispose();
      });
    }
    void Set3thTargetGameTrigger() {
      IDisposable disp = mTargetManager.onTargetDestroy.Subscribe(_ =>
      {
        while (mTargetManager.num <= 3) mTargetManager.Create3thTarget();
      });
      mTimer.whenTimeIsUp.Subscribe(time => {
        disp.Dispose();
      });
    }

    void SetFlickGameTrigger() {
      IDisposable disp1 = mTargetManager.onTargetDestroy.Subscribe(_ =>{
        while (mTargetManager.num <= 1) mTargetManager.CreateFlickTarget();
      });
      mTimer.whenTimeIsUp.Subscribe(time => {
        disp1.Dispose();
      });
    }

    void SetTrackingGameTrigger() {
      IDisposable disp1 = mTargetManager.onTargetDestroy.Subscribe(_ => {
        while (mTargetManager.num <= 1) mTargetManager.CreateTrackingTarget();
      });
      mTimer.whenTimeIsUp.Subscribe(time => {
        disp1.Dispose();
      });
    }
    void Reset() {
      mShotToShot = 0;
      score = 0;
      rate = 100;
      hitCount = 0;
      shotCount = 0;
      mAimTime = 0;
      onTime = 0;
      offTime = 0;
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