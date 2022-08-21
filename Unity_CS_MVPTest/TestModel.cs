// using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Test {

    public class TestModel
    {
        ReactiveProperty<Vector3> m_rcVec3;
        // ReactiveProperty<Vector3> m_rcCameraVec3;

        public TestModel()
        {
            m_rcVec3 = new ReactiveProperty<Vector3>(Vector3.zero);
            // m_rcCameraVec3 = new ReactiveProperty<Vector3>(Vector3.zero);
        }

        public IReactiveProperty<Vector3> RcVec3 => m_rcVec3;
        // public IReactiveProperty<Vector3> RcCameraVec3 => m_rcCameraVec3;
    }
}