using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

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

    // Called when new player joins server
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform startPos = GetStartPositionForPlayer(conn); // Get available start position
        GameObject playerInstance = Instantiate(playerPrefab, startPos.position, startPos.rotation); // instantiate player at that position
        NetworkServer.AddPlayerForConnection(conn, playerInstance); // spawn player and associate it with the connection

        string currentScene = SceneManager.GetActiveScene().name;
        int playerMove = GetPlayerMove(currentScene);

        var player = conn.identity.GetComponent<PlayerAnimatorController>();
        if (player != null)
        {
            Debug.Log($"[Server] Setting PlayerMove = {playerMove} for player {conn.connectionId}");
            player.RpcForceAnimationState(playerMove); // call a ClientRpc to apply animation on client
        }
        else
        {
            Debug.LogWarning($"[Server] No PlayerAnimatorController found on player {conn.connectionId}");
        }
    }

    // Returns integer that represents Player's Move animation state
    private int GetPlayerMove(string sceneName)
    {
        switch (sceneName)
        {
            case "Room1_Sea":
                return 1; // IdleSit
            case "Room2_Beach":
                return 0; // IdleStand
            default:
                return 0; // Default IdleStand
        }
    }

    // Determins where a new player should spawn
    private Transform GetStartPositionForPlayer(NetworkConnectionToClient conn)
    {
        if (startPositions.Count == 0) // if no startPositions in list
        {
            Debug.LogWarning("No start positions found, defaulting to Vector3.zero");
            return new GameObject("FallbackSpawn").transform;
        }

        // Round-robin logic ensures each player has different spot
        int index = conn.connectionId % startPositions.Count; // select a unique spawn index based on player's connection ID (Player 0 -> index 0)
        return startPositions[index]; // return selected spawn Transform to use when instantiating the player
    }
}
