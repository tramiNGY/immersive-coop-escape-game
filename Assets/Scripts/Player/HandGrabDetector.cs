using UnityEngine;

public class HandGrabDetector : MonoBehaviour
{
    public GameObject currentGrabableObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        GrabbableObject grabbable = other.GetComponent<GrabbableObject>();

        if (grabbable != null)
        {
            currentGrabableObject = other.gameObject;
            Debug.Log("Grabbed object: " + currentGrabableObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentGrabableObject)
        {
            Debug.Log("Ungrabbed object: " + currentGrabableObject.name);
            currentGrabableObject = null;
        }
    }
}

