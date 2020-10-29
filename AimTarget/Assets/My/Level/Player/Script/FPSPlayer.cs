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
    public LineRenderer lineRenderer;
    public Transform shotT;
    public Transform cameraT;
    /*****private field*****/
    float mFwdSpeed;
    float mStfSpeed;
    RaycastHit mRaycastHit;
    MousePointee mBeforeMousePointee = null;
    /*****event field*****/
    public IObservable<Unit> onShot { get { return mShotSubject; } }
    protected Subject<Unit> mShotSubject = new Subject<Unit>();
    /*****monobehaviour method*****/
    void Update() {
      ComputeMousePoint();
      MouseEvent();
      if (Input.GetMouseButtonDown(0))
        Shot();
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
      if (mFwdSpeed * mFwdSpeed > 0.01f)
        pos += transform.forward * mFwdSpeed * dt;
      if (mStfSpeed * mStfSpeed > 0.01f)
        pos += transform.right * mStfSpeed * dt;
      transform.position = pos;
    }
    void CheckRotate() {
      float dt = Time.fixedDeltaTime;
      pitchSpeed = 200 * -Input.GetAxis("Mouse Y");
      yawSpeed = 200 * Input.GetAxis("Mouse X");
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
        mBeforeMousePointee = null;
        return;
      }
      MousePointee m = mRaycastHit.transform.GetComponent<MousePointee>();
      if (mBeforeMousePointee != null && mBeforeMousePointee != m)
        mBeforeMousePointee.offEvent.Invoke();
      if (m == null) {
        mBeforeMousePointee = null;
        return;
      }
      m.onEvent.Invoke();
      if (Input.GetMouseButton(0))
        m.clickEvent.Invoke();
      if (Input.GetMouseButtonDown(0))
        m.downEvent.Invoke();
      if (Input.GetMouseButtonUp(0))
        m.upEvent.Invoke();
      mBeforeMousePointee = m;
      return;
    }
    void Shot() {
      mShotSubject.OnNext(Unit.Default);
    }
  }
}

