using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
namespace My {
  public class FPSPlayer : MonoBehaviour {
    /*****public field*****/
    public float pitchSpeed { get; set; }
    public float yawSpeed { get; set; }
    public int dpi {
      get { return mDPI; }
      set {
        mDPI = Mathf.Clamp(value, 1, 100);
        PlayerPrefs.SetInt("DPI",mDPI);
      }
    }
    public LineRenderer lineRenderer;
    public Transform shotT;
    public Transform cameraT;
    public Timer FootstepsTimer;
    /*****private field*****/
    float mFwdSpeed;
    float mStfSpeed;
    int mDPI;
    RaycastHit mRaycastHit;
    MousePointee mMousePointee = null;
    MousePointee mBeforeMousePointee = null;
    /*****event field*****/
    public IObservable<Unit> onShot { get { return mShotSubject; } }
    protected Subject<Unit> mShotSubject = new Subject<Unit>();
    public IObservable<Unit> onMove { get { return mMoveSubject; } }
    protected Subject<Unit> mMoveSubject = new Subject<Unit>();
    /*****monobehaviour method*****/
    void Awake() {
      dpi = PlayerPrefs.GetInt("DPI", 20);
      FootstepsTimer.whenTimeIsUp.Subscribe(_ => SEManager.instance.Play("足音"));
    }
    void Update() {
      ComputeMousePoint();
      if (Input.GetMouseButtonDown(0))
        Shot();
      MouseEvent();
    }
    void FixedUpdate() {
      CheckMove();
      CheckRotate();
    }
    /*****public method*****/
    public void SetMouseCursorVisible(bool f) {
      if (f) {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
      } else {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
      }
    }
    /*****private method*****/
    void CheckMove() {
      float dt = Time.fixedDeltaTime;
      Vector3 pos = transform.position;
      mFwdSpeed = 0.0f; mStfSpeed = 0.0f;
      if (Input.GetKey(KeyCode.W))
        mFwdSpeed += 7.0f;
      if (Input.GetKey(KeyCode.S))
        mFwdSpeed -= 7.0f;
      if (Input.GetKey(KeyCode.A))
        mStfSpeed -= 5.0f;
      if (Input.GetKey(KeyCode.D))
        mStfSpeed += 5.0f;
      if (mFwdSpeed * mFwdSpeed > 0.01f) {
        pos += transform.forward * mFwdSpeed * dt;
        mMoveSubject.OnNext(Unit.Default);
        FootstepsTimer.Play();
      } else {
        FootstepsTimer.Stop();
      }
      if (mStfSpeed * mStfSpeed > 0.01f)
        pos += transform.right * mStfSpeed * dt;
      transform.position = pos;
    }
    void CheckRotate() {
      float dt = Time.fixedDeltaTime;
      pitchSpeed = dpi*10 * -Input.GetAxis("Mouse Y");
      yawSpeed = dpi*10 * Input.GetAxis("Mouse X");
      transform.rotation = Quaternion.AngleAxis(yawSpeed * dt, transform.up) * transform.rotation;
    }
    void ComputeMousePoint() {
      if (cameraT) {
        //lineRenderer.SetPosition(0, shotT.position);
        Physics.Raycast(cameraT.position, cameraT.forward, out mRaycastHit);
        //if (mRaycastHit.transform)
        //    lineRenderer.SetPosition(1, mRaycastHit.point);
        //else
        //    lineRenderer.SetPosition(1, cameraT.position + cameraT.forward * 10000f);
      }
    }
    void MouseEvent() {
      if (!mRaycastHit.transform) {
        mMousePointee = null;
        mBeforeMousePointee = null;
        return;
      }
      mMousePointee = mRaycastHit.transform.GetComponent<MousePointee>();
      if (mBeforeMousePointee != null && mBeforeMousePointee != mMousePointee)
        mBeforeMousePointee.offEvent.Invoke();
      if (mMousePointee == null) {
        mBeforeMousePointee = null;
        return;
      }
      mMousePointee.onEvent.Invoke();
      if (Input.GetMouseButton(0))
        mMousePointee.clickEvent.Invoke();
      if (Input.GetMouseButtonDown(0))
        mMousePointee.downEvent.Invoke();
      if (Input.GetMouseButtonUp(0))
        mMousePointee.upEvent.Invoke();
      mBeforeMousePointee = mMousePointee;
      return;
    }
    void Shot() {
      mShotSubject.OnNext(Unit.Default);
      SEManager.instance.Play("ハンドガン");
    }
  }
}

