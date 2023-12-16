using System;
using UnityEngine;
using static Scripts.Scene.SceneUtility.SceneName;

namespace Scripts.Scene
{
    public class BaseScene : MonoBehaviour
    {
        #region Fields

        private bool _initialized;

        #endregion
        private void Start()
        {
            Initialized();
        }

        protected virtual bool Initialized()
        {
            if (_initialized) return false;
            _initialized = true;
            Main.Scene.LoadScene(IntroScene);
            return _initialized;
        }
    }
}