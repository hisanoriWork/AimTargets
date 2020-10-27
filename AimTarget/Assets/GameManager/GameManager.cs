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
        void Awake()
        {
            player.SetMouseCursorVisible(false);
            timer.Stop();
            timer.onDigitalTimeChanged.Subscribe(time =>{
                timeUI.text = time.ToString();
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

        public void Reset() {
            score = 0;
            rate = 100;
            hitCount = 0;
            shotCount = -1;
            timer.Reset();
            scoreUI.text = score.ToString();
            rateUI.text = ((int)rate).ToString() + "%";
        }

        public void StartGame()
        {
            player.onShot.Subscribe(_ => {
                shotCount++;
                rate = shotCount > 0 ? (float)hitCount / (float)shotCount * 100f : 100f;
                rateUI.text = ((int)rate).ToString() + "%";
            });
            targetManager.onTargetBreak.Subscribe(_ => {
                hitCount++;
                score += 100;
                scoreUI.text = score.ToString();
            });
            targetManager.CreateTarget();
            targetManager.CreateTarget();
            targetManager.CreateTarget();
        }
    }

}