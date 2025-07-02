using UnityEngine;

public class LanternBlueLight : MonoBehaviour
{
    [SerializeField] private Light LanternLight1;
    [SerializeField] private Light LanternLight2;
    [SerializeField] private Light LanternLight3;
    [SerializeField] private Light LanternLight4;
    [SerializeField] private Collider CompassCollider;
    [SerializeField] private Color MagicLanternLight = Color.blue;

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
        if (other == CompassCollider)
        {
            LanternLight1.color = MagicLanternLight;
            LanternLight1.intensity = 0.8f;
            LanternLight1.range = 1.5f;
            LanternLight2.color = MagicLanternLight;
            LanternLight2.intensity = 0.8f;
            LanternLight2.range = 1.5f;
            LanternLight3.color = MagicLanternLight;
            LanternLight3.intensity = 0.8f;
            LanternLight3.range = 1.5f;
            LanternLight4.color = MagicLanternLight;
            LanternLight4.intensity = 0.8f;
            LanternLight4.range = 1.5f;
            
        }
    }
}
