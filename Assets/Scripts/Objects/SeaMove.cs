using UnityEngine;
using Mirror;

// Move position translation of Sea and Scene Trigger
public class SeaMove : NetworkBehaviour
{
    [SerializeField] RuneSlotDetector runeEdetector;
    [SerializeField] RuneSlotDetector runeOdetector;
    [SerializeField] RuneSlotDetector runeRdetector;
    
    [SerializeField] GameObject sceneTriggerBeach;

    private bool hasMoved = false; // Check if movement has started
    private bool isMoving = false; // Indicate if movement in progress

    private Vector3 startSeaPos;
    private Vector3 targetSeaPos;
    private Vector3 startTriggerPos;
    private Vector3 targetTriggerPos;

    private float moveProgress = 0f; // Tracks interpolation factor between 0 and 1
    private float moveSpeed = 0.03f;

    public void Update()
    {
        if (!hasMoved && runeEdetector.hasFaded && runeOdetector.hasFaded && runeRdetector.hasFaded) // Move condition if all runes activated rune slot fade
        {
            SeaTranslate(); // Initialize movement positions
            isMoving = true; // start moving
            hasMoved = true; // prevent from moving again
        }

        if (isMoving)
        {
            moveProgress += Time.deltaTime * moveSpeed; // Increase interpolation progress over time
            transform.position = Vector3.Lerp(startSeaPos, targetSeaPos, moveProgress); // Move sea and trigger objects
            sceneTriggerBeach.transform.position = Vector3.Lerp(startTriggerPos, targetTriggerPos, moveProgress); // Lerp linear interpolation smooth transition

            if (moveProgress >= 1f) // Once movement completed
            {
                transform.position = targetSeaPos; // snap to final position
                sceneTriggerBeach.transform.position = targetTriggerPos;
                isMoving = false; // stop moving
            }
        }
    }

    // Set start and target positions for the movement
    public void SeaTranslate()
    {
        startSeaPos = transform.position; // stores current position at start
        targetSeaPos = transform.position + new Vector3(0, 0, -25f); // Move 25 units backward on Z-axis (visual effect to make Player believe move forward +Z)

        startTriggerPos = sceneTriggerBeach.transform.position;
        targetTriggerPos = sceneTriggerBeach.transform.position + new Vector3(0, 0, -25f);
        moveProgress = 0f; // reset progress begin interpolation
    }
}
