using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace my{
    public class FPSPlayer : MonoBehaviour
    {
        /*camera*/
        float fwdSp = 0.0f;//forwardSpeed
        float stfSp = 0.0f;//strafeSpeed
        float maxAS = Mathf.PI * 8;
        [System.NonSerialized] public float ps, ys; //PitchSpeed,YawSpeed
        /*jump*/
        public Rigidbody rb;
        private CapsuleCollider col;
        float jumpPower = 3.0f;
        float orgColHight;// col.Heiht
        private Vector3 orgVectColCenter; //col.Center
        public float velocity;// col.移動量
        public Curve jumpCurve;
        public float onGHeight = 0.2f; //onGroundHeight
        /*shot*/
        Transform shotT;
        public Transform cameraT;
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shot();
            }
        }
        void FixedUpdate()
        {
            CheckMove();
            CheckRotate();
        }
        void CheckMove()
        {
            float dt = Time.fixedDeltaTime;
            Vector3 pos = transform.position;
            fwdSp = 0.0f; stfSp = 0.0f;
            if (Input.GetKey(KeyCode.W))
                fwdSp += 7.0f;
            if (Input.GetKey(KeyCode.S))
                fwdSp -= 7.0f;
            if (Input.GetKey(KeyCode.A))
                stfSp -= 3.0f;
            if (Input.GetKey(KeyCode.D))
                stfSp += 3.0f;
            if (fwdSp * fwdSp > 0.01f)
                pos += transform.forward * fwdSp * dt;
            if (stfSp * stfSp > 0.01f)
                pos += transform.right * stfSp * dt;
            transform.position = pos;
        }
        

        void CheckRotate() {
            float dt = Time.fixedDeltaTime;
            ps = 1000 * -Input.GetAxis("Mouse Y");
            ys = 1000 * Input.GetAxis("Mouse X");
            transform.rotation = Quaternion.AngleAxis(ys * dt, transform.up) * transform.rotation;
        }


        void Shot()
        {

            RaycastHit hit;
            if (cameraT)
            {
                if (Physics.Raycast(cameraT.position, cameraT.forward, out hit))
                {
                    TargetC target = hit.transform.GetComponent<TargetC>();
                    if (target != null)
                    {
                        target.Damage(10);
                    }
                }
                Debug.DrawRay(cameraT.position, cameraT.forward * hit.distance, Color.yellow);
            }
            
        }
        //void Jump()
        //{
        //    rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
        //}
        //void CheckOnGround()
        //{
        //    jumpCurve.AddTime(Time.fixedDeltaTime);
        //    float jumpHeight = jumpCurve.GetValue();
        //    rb.useGravity = false;
        //    /***接地処理***/
        //    Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
        //    RaycastHit hitInfo = new RaycastHit();
        //    // 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
        //    if (Physics.Raycast(ray, out hitInfo) && hitInfo.distance > onGHeight)
        //    {
        //        col.height = orgColHight - jumpHeight;          // 調整されたコライダーの高さ
        //        float adjCenterY = orgVectColCenter.y + jumpHeight;
        //        col.center = new Vector3(0, adjCenterY, 0);	// 調整されたコライダーのセンター
        //    }
        //    else
        //    {// 閾値よりも低い時には初期値に戻す（念のため）					
        //        ResetCollider();
        //        jumpCurve.ReSet();
        //        rb.useGravity = true;
        //    }
        //}

        //void ResetCollider()
        //{// コンポーネントのHeight、Centerの初期値を戻す
        //    col.height = orgColHight;
        //    col.center = orgVectColCenter;
        //}
    }

}

