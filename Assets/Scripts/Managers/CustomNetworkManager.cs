using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    private NetworkSceneTrigger sceneTrigger;

    // When server has changed scene, reset flag
    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName); // base to extend method from parent class NetworkManager, not replace completely on override

        sceneTrigger = FindFirstObjectByType<NetworkSceneTrigger>();

        // Checks if SceneTrigger found
        if (sceneTrigger != null)
        {
            sceneTrigger.ResetSceneChangingFlag(); // Reset flag for changing scene
            Debug.Log("Changing scene flag reset");
        }
        else
        {
            Debug.LogWarning("No SceneTrigger found in scene");
        }
    }
}
