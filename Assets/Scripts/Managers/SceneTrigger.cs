using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public string newScene;
    private SceneLoader sceneLoader;

    public void Start()
    {
        sceneLoader = FindAnyObjectByType<SceneLoader>(); // Search for the SceneLoader from all scenes common SceneManager
    }

    public void TriggerSceneChange(string newScene)
    {
        if (sceneLoader != null)
        {
            sceneLoader.LoadScene(newScene);
        }
    }

    public void OnTriggerEnter(Collider other) // triggers scene change if player collider enters triggering object collider
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"[SceneTrigger] Triggered by: {gameObject.name}, loading scene: {newScene}");
            TriggerSceneChange(newScene);
        }
    }
}
