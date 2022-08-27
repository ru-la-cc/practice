using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class TestView : MonoBehaviour
    {
        [SerializeField, Tooltip("ATKボタン")]
        Button buttonHanakaya;

        [SerializeField, Tooltip("操作対象のモデル")]
        GameObject character;

        [SerializeField, Tooltip("アニメーションコントローラ")]
        Animator animator;

        [SerializeField, Tooltip("デバッグ用")]
        Text debugText;

        [SerializeField, Tooltip("ミク追従カメラ")]
        Camera mikuCamera;

        [SerializeField, Tooltip("カメラオフセット")]
        Vector3 cameraPos;

        [SerializeField, Header("ヒャッハー！")]
        GameObject hyahha;

        public Button HanayamButton => buttonHanakaya;
        public GameObject CharaModel => character;
        public Animator AnimatorController => animator;
        public Text DebugText => debugText;
        public Camera MikuCamera => mikuCamera;
        public Vector3 CameraPos => cameraPos;
        public GameObject Hyahha => hyahha;

        public Action<Vector3> OnCameraPositionChangeHandler;
        public Action OnHanayamaAttackHandler;
        public Action<Collision> HitHanayamaPunchHandler;

        public void SetIdleState(bool isIdle) => animator.SetBool("IsIdle", isIdle);

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void OnHitHanayamaPunch(Collision other)
        {
            HitHanayamaPunchHandler?.Invoke(other);
        }

        public void OnCameraPositionChanced(Vector3 vec3)
        {
             OnCameraPositionChangeHandler?.Invoke(vec3);
        }

        public void OnHanayamaAttacked()
        {
            OnHanayamaAttackHandler?.Invoke();
        }

    }
}
