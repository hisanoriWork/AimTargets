using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
namespace My {
  public class FPSPlayer : MonoBehaviour {
    /*****public field*****/
    public float fwdSpeed;
    public float stfSpeed;
    public float backSpeed;
    public float pitchSpeed { get; set; }
    public float yawSpeed { get; set; }
    public Vector3 raycastHitPos { get {
        if (mRaycastHit.transform) return mRaycastHit.point;
        return Vector3.zero;
    }}
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
    public SimpleShoot simpleShoot;
    public bool useLaser = true;
    /*****private field*****/
    
    int mDPI;
    RaycastHit mRaycastHit;
    MousePointee mMousePointee = null;
    MousePointee mBeforeMousePointee = null;
    /*****event field*****/
    public IObservable<Unit> onShot { get { return mShotSubject; } }
    protected Subject<Unit> mShotSubject = new Subject<Unit>();
    public IObservable<Unit> onMove { get { return mMoveSubject; } }
    protected Subject<Unit> mMoveSubject = new Subject<Unit>();
    public IObservable<Unit> onOff { get { return mOffSubject; } }
    protected Subject<Unit> mOffSubject = new Subject<Unit>();
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
      int fwd = 0, stf = 0;
      float xSpeed = 0.0f;
      bool isMoving = false;
      /*input*/
      if (Input.GetKey(KeyCode.W))
        fwd += 1;
      if (Input.GetKey(KeyCode.S))
        fwd -= 1;
      if (Input.GetKey(KeyCode.A))
        stf -= 1;
      if (Input.GetKey(KeyCode.D))
        stf += 1;
      /*前後の速さ決定*/
      if (fwd > 0) xSpeed = fwdSpeed;
      else if (fwd == 0) xSpeed = 0f;
      else xSpeed = backSpeed;
      /*移動*/
      if (fwd * fwd > 0 && stf * stf > 0) {
        pos += (float)fwd * transform.forward * Mathf.Lerp(xSpeed, stfSpeed, 0.5f) * dt;
        pos += stf * transform.right * Mathf.Lerp(xSpeed, stfSpeed, 0.5f) * dt;
        isMoving = true;
      } else if(fwd != 0 && stf == 0){
        pos += fwd * transform.forward * xSpeed * dt;
        isMoving = true;
      } else if (fwd == 0 && stf != 0){
        pos += stf* transform.right * stfSpeed* dt;
        isMoving = true;
      }
      if (isMoving) {
        transform.position = pos;
        mMoveSubject.OnNext(Unit.Default);
        FootstepsTimer.Play();
        isMoving = false;
      } else {
        FootstepsTimer.Stop();
      }

    } 
    void CheckRotate() {
      float dt = Time.fixedDeltaTime;
      pitchSpeed = dpi*10 * -Input.GetAxis("Mouse Y");
      yawSpeed = dpi*10 * Input.GetAxis("Mouse X");
      transform.rotation = Quaternion.AngleAxis(yawSpeed * dt, transform.up) * transform.rotation;
    }
    void ComputeMousePoint() {
      if (cameraT) {
        Physics.Raycast(cameraT.position, cameraT.forward, out mRaycastHit);
      }
      if (lineRenderer && useLaser) {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, shotT.position);
        if (mRaycastHit.transform)
          lineRenderer.SetPosition(1, mRaycastHit.point);
        else
          lineRenderer.SetPosition(1, cameraT.position + cameraT.forward * 10000f);
      }
    }
    void MouseEvent() {
      if (!mRaycastHit.transform) {
        mMousePointee = null;
        mBeforeMousePointee = null;
        return;
      }
      mMousePointee = mRaycastHit.transform.GetComponent<MousePointee>();
      if (mBeforeMousePointee != null && mBeforeMousePointee != mMousePointee) {
        mBeforeMousePointee.offEvent.Invoke();
        mOffSubject.OnNext(Unit.Default);
      }
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
      simpleShoot.ToShootAnimation();
      mShotSubject.OnNext(Unit.Default);
      SEManager.instance.Play("ハンドガン");
    }
  }
}

