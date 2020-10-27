using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace my
{
    public class FPSCamera : MonoBehaviour
    {
        public Transform c; //camera
        public FPSPlayer player;
        [System.NonSerialized] public float pitchSpeed; //PitchSpeed
        [System.NonSerialized] public float maxPitch;
        [System.NonSerialized] public float pitch;
        public Vector3 cameraHeight;
        void Start(){
            maxPitch = 45;
     
        }
        void FixedUpdate()
        {
            Transform tf = player.transform;
            float pitchSpeed = player.ps;
            float dt = Time.fixedDeltaTime;
            Vector3 cameraPos = tf.position + cameraHeight;
            pitch += pitchSpeed * dt;
            pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);
            Quaternion PitchQ = Quaternion.AngleAxis(pitch, tf.right);
            Vector3 viewForward = PitchQ * tf.forward;
            //Vector3 up = PitchQ * Vector3.forward;
            c.rotation = Quaternion.LookRotation(viewForward);               
            c.position = cameraPos;
        }
    }
}
