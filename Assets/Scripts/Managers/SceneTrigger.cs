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

    public void ColliderTriggerSceneChange(Collider other) // triggers scene change if player collider enters triggering object collider
    {
        if (other.CompareTag("Player"))
        {
            TriggerSceneChange(newScene);
        }
    }
}
