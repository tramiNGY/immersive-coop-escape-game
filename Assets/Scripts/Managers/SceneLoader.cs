using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Image SceneTransitionImage;
    private string currentScene;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Make SceneManager object persistent in all scenes
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string newScene)
    {
        if (SceneManager.GetSceneByName(newScene).isLoaded) // prevents reloading same scene
        {
            Debug.Log($"Scene {newScene} is already loaded");
            return;
        }

        SceneTransitionImage.gameObject.SetActive(true);
        SceneTransitionImage.color = new Color(0, 0, 0, 1f); // RGB + alpha: 0 transparent -> 1 opaque

        // Start loading new scene asynchronously in additive mode
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

        // When new scene has finished loading
        loadOp.completed += (AsyncOperation op) =>
        {
            if (!string.IsNullOrEmpty(currentScene)) // of there is a previous scene
            {
                SceneManager.UnloadSceneAsync(currentScene); // unload scene asynchronously
            }

            currentScene = newScene; // update currentscene to new scene

            SceneTransitionImage.gameObject.SetActive(false); // disable transition image to see new scene
        };
    }
}
