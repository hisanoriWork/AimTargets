using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UnityEngine.UI;

namespace my {
    public class GameManager : MonoBehaviour
    {
        public TargetManager targetManager;
        public Timer timer;
        public Text timeUI;
        public Text scoreUI;
        public Text rateUI;
        public FPSPlayer player;
        public float rate = 100;
        public int shotCount = 0;
        public int hitCount = 0;
        public int score = 0;
        public GameObject startScreen;
        public GameObject resultScreen;
        public ScoreRate sr;
        public Canvas gameCanvas;
        public ResultController resultCon;
        void Awake()
        {
            //startScreen.SetActive(true);
            player.SetMouseCursorVisible(false);
            timer.Stop();
            timer.onDigitalTimeChanged.Subscribe(time =>{
                timeUI.text = time.ToString();
            });
            timer.whenTimeIsUp.Subscribe(time => {
                resultScreen.SetActive(true);
                targetManager.ClearTarget();
                timer.Stop();
            });
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Escape)) Quit();
        }

        void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
#endif
        }

        public void StartGame()
        {
            Reset();
            timer.Play();
            IDisposable disp1;
            IDisposable disp2;
            disp1 = player.onShot.Subscribe(_ => {
                shotCount++;
                rate = shotCount > 0 ? (float)hitCount / (float)shotCount * 100f : 100f;
                rateUI.text = ((int)rate).ToString() + "%";
            });
            disp2 = targetManager.onTargetBreak.Subscribe(_ => {
                hitCount++;
                score += 100;
                scoreUI.text = score.ToString();
            });
            timer.whenTimeIsUp.Take(1).Subscribe(time => {resultCon.Register(score, rate);});
            timer.whenTimeIsUp.Subscribe(time => {
                disp1.Dispose();
                disp2.Dispose();
            });
            targetManager.CreateTarget();
            targetManager.CreateTarget();
            targetManager.CreateTarget();


        }

        void Reset()
        {
            score = 0;
            rate = 100;
            hitCount = 0;
            shotCount = -1;
            timer.Reset();
            scoreUI.text = score.ToString();
            rateUI.text = ((int)rate).ToString() + "%";
        }
    }

}