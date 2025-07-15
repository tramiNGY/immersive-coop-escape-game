using UnityEngine;
using Mirror;

// Handles scene transition trigger
public class NetworkSceneTrigger : NetworkBehaviour
{
    [SerializeField] private string newScene;

    // Flag to avoid changing scene multiple times
    private static bool sceneChanging = false;

    private void OnTriggerEnter(Collider other)
    {
        // CHeck if object is a player
        if (!other.CompareTag("Player")) return;

        // If loading scene already happening, do nothing
        if (sceneChanging)
        {
            Debug.LogWarning("Loading scene currently happening");
            return;
        }

        if (isServer)
        {
            sceneChanging = true; // change scene in progress

            foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
            {
                if (conn.identity != null)
                {
                    Debug.Log($"Destroying player for conn {conn.connectionId}");
                    NetworkServer.Destroy(conn.identity.gameObject); // delete all current scene players to respawn them in newscene
                }
            }
            NetworkManager.singleton.ServerChangeScene(newScene); // server change scene and sync on all clients
        }

        else if (isClient)
        {
            CmdChangeScene(newScene); // Ask Server to change scene
        }
    }

    [Command]
    private void CmdChangeScene(string sceneName) // command sent from client to server
    {
        if (sceneChanging) return; // Check if not currently  changing scene

        sceneChanging = true;

        if (NetworkServer.active)
        {
            foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
            {
                if (conn.identity != null)
                {
                    Debug.Log($"Destroying player for conn {conn.connectionId}");
                    NetworkServer.Destroy(conn.identity.gameObject);
                }
            }

            NetworkManager.singleton.ServerChangeScene(sceneName);
        }
    }

    // Method called automatically by server after Mirror done loading scene
    public void ResetSceneChangingFlag()
    {
        sceneChanging = false; // reset flag to enable other scene change
    }
}
