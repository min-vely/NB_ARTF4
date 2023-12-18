using System;
using Scripts.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Scripts.Scene
{
    public class BaseScene : MonoBehaviour
    {
        #region Fields

        private bool _initialized;
        [SerializeField] protected GameObject eventSystem;

        #endregion

        #region Properties

        public BaseScene CurrentScene { get; set; }

        #endregion
        private void Start()
        {
            Initialized();
        }

        protected virtual bool Initialized()
        {
            if (_initialized) return false;
            _initialized = true;
            Main.Scene.CurrentScene = this;
            // SceneUtility.LoadScene(SceneUtility.SceneName.IntroScene);
            Object systemObject = FindObjectOfType<EventSystem>();
            if (systemObject == null)Instantiate(eventSystem).name = "@EventSystem";
            return _initialized;
        }
    }
}