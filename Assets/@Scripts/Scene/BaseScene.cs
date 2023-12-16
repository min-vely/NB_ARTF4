using System;
using Scripts.Utility;
using UnityEngine;
using static Scripts.Utility.SceneUtility.SceneName;

namespace Scripts.Scene
{
    public class BaseScene : MonoBehaviour
    {
        #region Fields

        private bool _initialized;

        #endregion

        #region Properties


        #endregion
        
        private void Start()
        {
            Initialized();
        }

        protected virtual bool Initialized()
        {
            if (_initialized) return false;
            _initialized = true;
            SceneUtility.LoadScene(Intro);
            return _initialized;
        }
    }
}