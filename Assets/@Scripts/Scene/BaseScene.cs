using System;
using Scripts.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Scripts.Scene
{
    public enum Label
    {
        PreLoadScene,
        IntroScene,
        LoadingScene,
        GameScene,
        SampleScene
    }
    public abstract class BaseScene : MonoBehaviour
    {
        #region Fields

        private bool _initialized;
        private Label _sceneLabel;

        #endregion

        #region Properties
        public Label CurrentScene
        {
            get
            {
                Debug.Log($"Get Label {_sceneLabel.ToString()}");
                return _sceneLabel;
            }
            set
            {
                _sceneLabel = value;
                Debug.Log($"Set Label {_sceneLabel.ToString()}");
            }
        }

        #endregion

        private void Awake()
        {
            if (Main.Resource.LoadBase)
            {
                // TODO : 로드가 되어있다면, 추가적인 초기화 필요
                Initialized();
            }
            else
            {
                Main.Resource.AllLoadAsync<Object>($"Preload", (key, count, totalCount) =>
                {
                    Debug.Log($"[BaseScene] Load asset {key} ({count}/{totalCount})");
                    if (count < totalCount) return;
                    Main.Resource.LoadBase = true;
                    // TODO : 추가적인 초기화 필요
                    Initialized();
                });
            }
        }

        protected virtual bool Initialized()
        {
            if (_initialized) return false;
            _initialized = true;
            Object eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null) Main.Resource.InstantiatePrefab("EventSystem.prefab").name = "@EventSystem";
            return _initialized;
        }
    }
}