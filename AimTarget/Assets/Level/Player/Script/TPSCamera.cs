using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My {
  public class TPSCamera : MonoBehaviour {
    /*****public field*****/
    public Transform c; //cameraTransform
    public Transform t;
    public Vector3 fwd;
    public Vector3 angle;
    public float pitchSpeed = 1f; //pitchSpeed
    public float maxPitch = Mathf.PI / 3.0f;
    public float pitch = 0.0f;
    /*****private field*****/
    private float mTDist = 2.0f; //mTargetDistance
    private float mHDist = 5.0f; // mHorizenDistance
    private float mVDist = 2.0f; // mVerticalDistance
    private float mK = 1000.0f; //mSpringConstant
    private Vector3 mAPos; //mActualPosition
    private Vector3 mVel; //mVelocity
    /*****monobehaviour method*****/
    void Start() {
      c.position = ComputeCameraPos();
      fwd = Vector3.forward;
    }
    public void FixedUpdate() {
      Vector3 iPos = ComputeCameraPos(); //idealPosition
      Vector3 target = t.position + fwd * mTDist;
      Vector3 diff = mAPos - iPos; // difference
      Vector3 acel = -mK * diff - 2.0f * Mathf.Sqrt(mK) * mVel;
      mVel += acel * Time.fixedDeltaTime;
      mAPos += mVel * Time.fixedDeltaTime;
      c.rotation = Quaternion.LookRotation(target - mAPos);
      c.position = mAPos;
    }
    /*****public method*****/
    /*****private method*****/
    Vector3 ComputeCameraPos() {
      Vector3 iPos = t.position;
      iPos -= fwd * mHDist;
      iPos += Vector3.up * mVDist;
      return iPos;
    }
  }
}
