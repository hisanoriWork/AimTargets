using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveC : MonoBehaviour
{
    /*****public field*****/
    public Transform t;
    public Vector3 vel{
        set {
            mVel = value;
        }
        get { return mVel; }
    }
    public Quaternion q{
        set{
            mQ = value;
            mQ.Normalize();
        }
        get { return mQ; }
    }
    /*****private field*****/
    Vector3 mVel;
    Quaternion mQ;
    float mVelTh;
    float mQTh;

    /*****monobehaviour method*****/

    void Awake(){
        mQ = Quaternion.identity;
        mVel = Vector3.zero;
        mVelTh = 0.01f;
        mQTh = 0.01f;
    }
    void FixedUpdate(){
        if (mVel.magnitude > mVelTh) {
            t.Translate(mVel);
        }
        if (Mathf.Acos(mQ.x *2) > mQTh){
            t.rotation = mQ * t.rotation;
        }
        mQ = Quaternion.identity;
        mVel = Vector3.zero;
    }
}
