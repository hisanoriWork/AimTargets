﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.Events;

namespace My {
  public class Timer : MonoBehaviour {
    [System.Serializable]
    public class MyIntEvent : UnityEvent<int> {
    }

    [System.Serializable]
    public class MyFloatEvent : UnityEvent<float> {
    }
    public MyIntEvent onDigitalCountEvent;
    public MyFloatEvent onAnalogCountEvent;
    public UnityEvent onTimeUpEvent;
    [Range(0, 1000)] public float time;
    public bool isReturning = true;
    public bool isRooping = false;
    public float analogTime { get { return m_analogTime; } }
    public int digitalTime { get { return m_digitalTime; } }
    public float rate { get { return m_digitalTime / m_time; } }
    public bool isPlaying { get; set; } = true;
    public IObservable<int> onDigitalTimeChanged { get { return digitalSubject; } }
    public IObservable<float> onAnalogTimeChanged { get { return analogSubject; } }
    public IObservable<Unit> whenTimeIsUp { get { return isUpSubject; } }
    /*****protected field*****/
    private Subject<int> digitalSubject = new Subject<int>();
    private Subject<float> analogSubject = new Subject<float>();
    private Subject<Unit> isUpSubject = new Subject<Unit>();
    private float m_time;
    private float m_analogTime = 100f;
    private int m_digitalTime = 100;
    /*****monoBehaviour method*****/
    void Awake() {
      m_time = time;
      m_analogTime = time;
      m_digitalTime = (int)time;
    }
    void FixedUpdate() {
      if (isPlaying & m_analogTime > 0f) {
        m_analogTime += -Time.fixedDeltaTime;
        analogSubject.OnNext(m_analogTime);
        onAnalogCountEvent.Invoke(m_analogTime);
        if (m_digitalTime != (m_digitalTime = (int)m_analogTime)) {
          digitalSubject.OnNext(m_digitalTime);
          onDigitalCountEvent.Invoke(m_digitalTime);
        }
        if (m_analogTime <= 0f) {
          isUpSubject.OnNext(Unit.Default);
          onTimeUpEvent.Invoke();
          Stop();
          if (isReturning) {
            Reset();
            if (isRooping) Play();
          }
        }
      }
    }
    /*****public method*****/
    public void Stop() {
      isPlaying = false;
    }
    public void Play() {
      isPlaying = true;
    }
    public void Reset() {
      m_analogTime = m_time;
      m_digitalTime = (int)m_time;
    }
  }
}