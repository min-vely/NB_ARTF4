using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    static string nextScene;

    [SerializeField] Image progressBar;
    [SerializeField] GameObject loadEndText;
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        StartCoroutine(LoadSceneProgress());
    }

    IEnumerator LoadSceneProgress()
    {

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {

            yield return null;

            if (op.progress < 0.6f) 
            {
                progressBar.fillAmount = op.progress;
            }
            else 
            {

                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.6f, 1f, timer);  
                if (progressBar.fillAmount >= 1f)
                {
                    loadEndText.SetActive(true);

                    

                    while (true)
                    {
                        if (Input.anyKeyDown)
                        {
                            op.allowSceneActivation = true;
                            yield break;
                        }
                        yield return null;
                    }
                }
            }

        }
    }
}