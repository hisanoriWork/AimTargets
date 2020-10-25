using UnityEngine;
using System.Collections;

namespace my
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour {
        /*****public field*****/
        public Vector3 ang;
        public float animSpeed = 1.5f;              // アニメーション再生速度設定
        public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
        public bool useCurves = true;               // Mecanimでカーブ調整を使うか設定する
        public float useCurvesHeight = 0.5f;        // カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）
        //public float forwardSpeed = 7.0f;
        //public float backwardSpeed = 2.0f;
        //public float speed = 7.0f;
        //public float rotateSpeed = 2.0f;
        public float jumpPower = 3.0f;
        /*****hoge*****/
        public Transform shotTransform;
        public FPSCamera fpsCamera;
        /*****private field*****/
        private CapsuleCollider col;
        private Rigidbody rb;
        private Vector3 velocity;
        private float orgColHight;
        private Vector3 orgVectColCenter;
        private Animator anim;                          // キャラにアタッチされるアニメーターへの参照
        private AnimatorStateInfo currentBaseState;         // base layerで使われる、アニメーターの現在の状態の参照
        private GameObject cameraObject;    // メインカメラへの参照
        /*****static field*****/
        static int idleState = Animator.StringToHash("Base Layer.Idle");
        static int locoState = Animator.StringToHash("Base Layer.Locomotion");
        static int jumpState = Animator.StringToHash("Base Layer.Jump");
        static int restState = Animator.StringToHash("Base Layer.Rest");
        /*****monobehavior method*****/
        void Start() {
            anim = GetComponent<Animator>();
            col = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            cameraObject = GameObject.FindWithTag("MainCamera");
            // CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
            orgColHight = col.height;
            orgVectColCenter = col.center;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shot();
            }
        }
        void FixedUpdate() {
            /***入力処理***/
            float h = Input.GetAxis("Horizontal");              // 入力デバイスの水平軸をhで定義
            float v = Input.GetAxis("Vertical");                // 入力デバイスの垂直軸をvで定
            anim.SetFloat("Speed", v);                          // Animator側で設定している"Speed"パラメタにvを渡す
            anim.SetFloat("Direction", h);                      // Animator側で設定している"Direction"パラメタにhを渡す
            anim.speed = animSpeed;                             // Animatorのモーション再生速度に animSpeedを設定する
            currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
            rb.useGravity = true;//ただしジャンプ中に重力を切る
            ///***移動処理***/
            //velocity = new Vector3(h, 0, v);
            //if (velocity.magnitude > 0.1)
            //{
            //    velocity.Normalize();
            //    velocity = transform.TransformDirection(velocity);// キャラクターのローカル空間での方向に変換
            //    velocity *= speed;
            //    transform.localPosition += velocity * Time.fixedDeltaTime;
            //}
            ///***旋回処理***/
            //Vector3 xang = Vector3.Scale(new Vector3(1f,0,1f),tps.fwd);
            //transform.rotation  = Quaternion.LookRotation(xang);
            ///***カメラ処理***/
            /***ジャンプ処理***/
            if (Input.GetButtonDown("Jump")) {
                if (currentBaseState.nameHash == locoState) {
                    if (!anim.IsInTransition(0)) {
                        rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                        anim.SetBool("Jump", true);     // Animatorにジャンプに切り替えるフラグを送る
                    }
                }
            }
            /***Locomotionステート***/
            if (currentBaseState.nameHash == locoState) {
                if (useCurves) {
                    resetCollider();
                }
            }
            /***Jumpステート***/
            else if (currentBaseState.nameHash == jumpState && !anim.IsInTransition(0)) {
                //cameraObject.SendMessage("setCameraPositionJumpView");  // ジャンプ中のカメラに変更
                /***カーブ調整***/
                if (useCurves) {
                    float jumpHeight = anim.GetFloat("JumpHeight"); //ジャンプ高さ：0～1
                    float gravityControl = anim.GetFloat("GravityControl"); //1->重力，0->無重力
                    if (gravityControl > 0)
                        rb.useGravity = false;
                    /***接地処理***/
                    Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
                    RaycastHit hitInfo = new RaycastHit();
                    // 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
                    if (Physics.Raycast(ray, out hitInfo)) {
                        if (hitInfo.distance > useCurvesHeight) {
                            col.height = orgColHight - jumpHeight;          // 調整されたコライダーの高さ
                            float adjCenterY = orgVectColCenter.y + jumpHeight;
                            col.center = new Vector3(0, adjCenterY, 0); // 調整されたコライダーのセンター
                        }
                        else {// 閾値よりも低い時には初期値に戻す（念のため）					
                            resetCollider();
                        }
                    }
                }
                // Jump bool値をリセットする（ループしないようにする）				
                anim.SetBool("Jump", false);
            }
            /***Idleステート***/
            else if (currentBaseState.nameHash == idleState)
            {
                if (useCurves) {//カーブでコライダ調整をしている時は、念のためにリセットする
                    resetCollider();
                }
                if (Input.GetButtonDown("Jump")) {//スペースでRestステート移行
                    anim.SetBool("Rest", true);
                }
            }
            /***Restステート***/
            else if (currentBaseState.nameHash == restState && !anim.IsInTransition(0)) {
                anim.SetBool("Rest", false);
            }

            
        }
        /*****private method*****/
        void resetCollider() {// コンポーネントのHeight、Centerの初期値を戻す
            col.height = orgColHight;
            col.center = orgVectColCenter;
        }

        void Shot()
        {
            RaycastHit hit;
            Transform c = fpsCamera.c;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(c.position, c.forward, out hit))
            {
                Debug.DrawRay(c.position, c.forward * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
            }
            else
            {
                Debug.DrawRay(c.position, c.forward * 1000, Color.white);
                Debug.Log("Did not Hit");
            }
        }
        /*****GUI method*****/
        void OnGUI()
        {
            GUI.Box(new Rect(Screen.width - 260, 10, 250, 150), "Interaction");
            GUI.Label(new Rect(Screen.width - 245, 30, 250, 30), "Up/Down Arrow : Go Forwald/Go Back");
            GUI.Label(new Rect(Screen.width - 245, 50, 250, 30), "Left/Right Arrow : Turn Left/Turn Right");
            GUI.Label(new Rect(Screen.width - 245, 70, 250, 30), "Hit Space key while Running : Jump");
            GUI.Label(new Rect(Screen.width - 245, 90, 250, 30), "Hit Spase key while Stopping : Rest");
            GUI.Label(new Rect(Screen.width - 245, 110, 250, 30), "Left Control : Front Camera");
            GUI.Label(new Rect(Screen.width - 245, 130, 250, 30), "Alt : LookAt Camera");
        }
    }
}
