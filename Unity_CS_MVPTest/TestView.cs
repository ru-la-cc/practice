using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class TestView : MonoBehaviour
    {
        [Tooltip("ATKボタン")]
        public Button buttonHanakaya;

        [Tooltip("操作対象のモデル")]
        public GameObject character;

        [Tooltip("アニメーションコントローラ")]
        public Animator animator;

        [Tooltip("デバッグ用")]
        public Text debugText;

        [Tooltip("ミク追従カメラ")]
        public Camera mikuCamera;

        [Tooltip("カメラオフセット")]
        public Vector3 cameraPos;

        public Camera mCamera => mikuCamera;

        public Action<Vector3> OnCameraPositionChangeHandler;
        public Action OnHanayamaAttackHandler;

        public void SetIdleState(bool isIdle) => animator.SetBool("IsIdle", isIdle);

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
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
