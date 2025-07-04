using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public string newScene;
    public SceneLoader sceneLoader;

    public void TriggerSceneChange(string newScene)
    {
        if (sceneLoader != null)
        {
            sceneLoader.LoadScene(newScene);
        }
    }
}
