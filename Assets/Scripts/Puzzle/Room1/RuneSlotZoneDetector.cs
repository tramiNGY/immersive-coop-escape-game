using UnityEngine;

public class RuneSlotDetector : MonoBehaviour
{
    [SerializeField] private Transform zoneCenter; // position of RuneSlot
    public float detectionRadius = 0.09f; // Detection Radius (in meter)
    public Transform runeTransform; // Rune cube that will enter the detection zone

    private bool hasFaded = false; // To avoid relaunching fade multiple times

    void Update()
    {
        if (hasFaded) 
            return; // If fade already started, do nothing

        float distance = Vector3.Distance(runeTransform.position, zoneCenter.position);
        if (distance < detectionRadius)
        {
            Debug.Log($"Rune {runeTransform.gameObject.name} is in the detection zone RuneSlot {zoneCenter.gameObject.name}");
            hasFaded = true;
            GetComponent<RuneSlotFade>().StartFade();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(zoneCenter.position, detectionRadius); // Visualize detection zone sphere
    }
}
