using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace my{
    public class FPSPlayer : MonoBehaviour
    {
        public float fwdSp = 0.0f;//forwardSpeed
        public float stfSp = 0.0f;//strafeSpeed
        public float maxAS = Mathf.PI * 8;
        public float ps, ys; //PitchSpeed,YawSpeed
        void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;
            Vector3 pos = transform.position;
            fwdSp = 0.0f;stfSp = 0.0f;
            if (Input.GetKey(KeyCode.W))
                fwdSp += 7.0f;
            if (Input.GetKey(KeyCode.S))
                fwdSp -= 7.0f;
            if (Input.GetKey(KeyCode.A))
                stfSp -= 3.0f;
            if (Input.GetKey(KeyCode.D))
                stfSp += 3.0f;
            if (fwdSp*fwdSp > 0.01f)
                pos += transform.forward * fwdSp * Time.fixedDeltaTime;
            if (stfSp*stfSp > 0.01f)
                pos += transform.right * stfSp * Time.fixedDeltaTime;
            transform.position = pos;
            ps = 1000 * -Input.GetAxis("Mouse Y");
            ys = 1000 * Input.GetAxis("Mouse X");
            transform.rotation = Quaternion.AngleAxis(ys * dt, transform.up) * transform.rotation;
        }
    }
}
