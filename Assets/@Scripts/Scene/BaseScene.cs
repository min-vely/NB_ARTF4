using Scripts.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Scripts.Scene
{
    public enum Label
    {
        IntroScene,
        LoadingScene,
        GameScene,
    }
    public abstract class BaseScene : MonoBehaviour
    {
        #region Properties
        public Label CurrentScene { get; set; }
        //public SoundManager SoundManagerInstance { get; set; }

        #endregion

        private void Awake()
        {
            if (Main.Resource.LoadBase) Initialized();
            else
            {
                Main.Resource.AllLoadAsync<Object>("Preload", (key, count, totalCount) =>
                {
                    Debug.Log($"[BaseScene] Load asset {key} ({count}/{totalCount})");
                    if (count < totalCount) return;
                    Main.Resource.LoadBase = true;
                    Initialized();
                });
            }
        }

        protected virtual bool Initialized()
        {

            Object eventSystem = FindObjectOfType<EventSystem>();
            Object sound = FindObjectOfType<SoundManager>();
            if (eventSystem == null) Main.Resource.InstantiatePrefab("EventSystem.prefab").name = "@EventSystem";
            if (sound == null)
            { 
                GameObject soundObject = Main.Resource.InstantiatePrefab("SoundManager.prefab");
                soundObject.name = "@SoundManager";
                SoundManager soundManager = SceneUtility.GetAddComponent<SoundManager>(soundObject);

                //SoundManagerInstance = soundManager;

                Debug.Log($"soundName : {soundManager}");
                AudioClip clip = Main.Resource.Load<AudioClip>("LoadBGM1.clip");
                Debug.Log($"Clip : {clip}");
                Debug.Log("노래 재생");
                soundManager.StartBGM();
                soundManager.PlayBGM(clip.name);

            }
            Main.Item.Initialized();
            return true;
        }
    }
}