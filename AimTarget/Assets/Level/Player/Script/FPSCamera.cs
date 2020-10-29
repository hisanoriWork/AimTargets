using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My {
  public class FPSCamera : MonoBehaviour {
    /*****public field****/
    public Transform c;
    public FPSPlayer player;
    public Vector3 cameraHeight;
    public float pitchSpeed { get; set; }
    public float maxPitch { get; set; } = 80;
    public float pitch { get; set; }
    /*****monobehavior method*****/
    void FixedUpdate() {
      Transform tf = player.transform;
      float pitchSpeed = player.pitchSpeed;
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
