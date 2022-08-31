using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
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
        // bool m_isAttack = false;
        Types.PlayerState mikuState = Types.PlayerState.Idle; 


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
            if(!m_isIdle && Types.PlayerState.Hanayama != mikuState)
            {
                m_verocity.Set(m_Horizontal * 0.5f * Time.deltaTime, 0, m_Vertical * 0.5f * Time.deltaTime);
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
                if(mikuState != Types.PlayerState.Hanayama){
                    view.OnHanayamaAttacked();
                    await AttackingAsync();
                }
            })
            .AddTo(gameObject);

            model.RcVec3
            .Subscribe(view.OnCameraPositionChanced)
            .AddTo(gameObject);

            view.Hyahha.OnCollisionEnterAsObservable()
            .Where(hit => hit.gameObject.tag == "MikuRightHand" && mikuState == Types.PlayerState.Hanayama)
            .Subscribe(hit =>
            {
                view.OnHitHanayamaPunch(hit);
            })
            .AddTo(gameObject);

            //Observable.EveryUpdate()
#if UNITY_EDITOR
            this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButton(0))
            .Select(_ => Input.mousePosition).Take(1)
            .Concat(this.UpdateAsObservable()
            .Select(_ => Input.mousePosition).Take(1))
            .Aggregate((pre, cur) => cur - pre)
            .RepeatUntilDestroy(this)
            .Subscribe(x => {
                view.MikuCamera.transform.RotateAround(view.CharaModel.transform.position, new Vector3(0,1,0), 1 * x.x);
                view.CameraPos = view.MikuCamera.transform.position - view.CharaModel.transform.position;
                view.CameraRota = view.MikuCamera.transform.rotation;
            })
            .AddTo(gameObject);
#else
            this.UpdateAsObservable()
            .Where(_ => Input.GetTouch(0).position)
            .Select(_ => Input.GetTouch(0).position).Take(1)
            .Concat(this.UpdateAsObservable()
            .Select(_ => Input.GetTouch(0).position).Take(1))
            .Aggregate((pre, cur) => cur - pre)
            .RepeatUntilDestroy(this)
            .Subscribe(x => Debug.Log($"swipe : x = {x}"))
            .AddTo(gameObject);
#endif
        }

        void OnHitHanayamaPunch(Collision other)
        {
            var hyahhabody = view.Hyahha.GetComponent<Rigidbody>();
            Vector3 hit = other.contacts[0].normal;
            hyahhabody.AddForce(hit*50, ForceMode.VelocityChange);
        }

        void OnPositionChange(Vector3 vec3)
        {
            view.DebugText.text = $"X:{vec3.x} Y:{vec3.y} Z:{vec3.z}";
            view.MikuCamera.transform.position = vec3 + view.CameraPos;
            view.MikuCamera.transform.rotation = view.CameraRota;
        }

        void OnHanayamaAttack()
        {
            view.AnimatorController.SetTrigger("TriggerAttack");
        }

        void SetHandler()
        {
            view.OnCameraPositionChangeHandler = OnPositionChange;
            view.OnHanayamaAttackHandler = OnHanayamaAttack;
            view.HitHanayamaPunchHandler = OnHitHanayamaPunch;
        }

        async UniTask AttackingAsync()
        {
            mikuState = Types.PlayerState.Hanayama;
            await UniTask.Delay(attack_wait);
            mikuState = Types.PlayerState.Idle;
        }
    }
}
