using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using UniRx;
// using UniRx.Triggers;
using Cysharp.Threading.Tasks;

namespace Test
{
    public class TestPresenter : MonoBehaviour
    {
        TestModel model;
        [SerializeField] TestView view;
        [SerializeField] float m_Horizontal;
        [SerializeField] float m_Vertical;

        bool m_isIdle = true;
        Vector3 m_verocity;
        [SerializeField, Tooltip("攻撃硬直時間(ms)")] const int attack_wait = 3500;
        bool m_isAttack = false;

        bool IsIdle() => Math.Abs(m_Horizontal) < 0.01f && Math.Abs(m_Vertical) < 0.01f;

        void Awake()
        {
            model = new TestModel();
            // model.RcCameraVec3.Value = view.mCamera.transform.position;
            InitControls();
            SetHandler();
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }
        
        void Move()
        {
            m_Horizontal = Input.GetAxis("Horizontal");
            m_Vertical = Input.GetAxis("Vertical");
            m_isIdle = IsIdle();
            if(!m_isIdle && !m_isAttack)
            {
                m_verocity.Set(m_Horizontal * 0.2f * Time.deltaTime, 0, m_Vertical * 0.2f * Time.deltaTime);
                view.CharaModel.transform.position += m_verocity;
                view.CharaModel.transform.rotation = Quaternion.LookRotation(m_verocity.normalized);
            }
            model.RcVec3.Value = view.CharaModel.transform.position;
            view.SetIdleState(m_isIdle);
        }

        void InitControls()
        {
            view.HanayamButton
            .OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromMilliseconds(1000))
            .Subscribe(async _ => {
                // Debug.Log("ボタン押した");
                if(!m_isAttack){
                    view.OnHanayamaAttacked();
                    await AttackingAsync();
                }
            });

            model.RcVec3
            .Subscribe(view.OnCameraPositionChanced)
            .AddTo(gameObject);
        }

        void OnPositionChange(Vector3 vec3)
        {
            view.DebugText.text = $"X:{vec3.x} Y:{vec3.y} Z:{vec3.z}";
            view.MikuCamera.transform.position = vec3 + view.CameraPos;
        }

        void OnHanayamaAttack()
        {
            view.AnimatorController.SetTrigger("TriggerAttack");
        }

        void SetHandler()
        {
            view.OnCameraPositionChangeHandler = OnPositionChange;
            view.OnHanayamaAttackHandler = OnHanayamaAttack;
        }

        async UniTask AttackingAsync()
        {
            m_isAttack = true;
            await UniTask.Delay(attack_wait);
            m_isAttack = false;
        }
    }
}
