using UnityEngine;

public class FollowFromAbove : MonoBehaviour
{
    public Transform target;
    public float heightOffset = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // Follow target's X and Z, keep a fixed Y (above)
        Vector3 newPosition = new Vector3(
            target.position.x,
            target.position.y + heightOffset,
            target.position.z
        );

        transform.position = newPosition;
    }
}
