using UnityEngine;

public class LanternBlueLight : MonoBehaviour
{
    [SerializeField] private Light LanternLight1;
    [SerializeField] private Light LanternLight2;
    [SerializeField] private Light LanternLight3;
    [SerializeField] private Light LanternLight4;
    [SerializeField] private Collider CompassCollider;
    [SerializeField] private Color MagicLanternLight = Color.blue;
    [SerializeField] private float LanternTransitionSpeed = 1f;
    private bool LanternChangeColor = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LanternChangeColor)
        {
            LanternLight1.color = Color.Lerp(LanternLight1.color, MagicLanternLight, Time.deltaTime * LanternTransitionSpeed);
            LanternLight1.intensity = Mathf.Lerp(LanternLight1.intensity, 0.8f, Time.deltaTime * LanternTransitionSpeed);
            LanternLight1.range = Mathf.Lerp(LanternLight1.range, 2f, Time.deltaTime * LanternTransitionSpeed);
            LanternLight2.color = Color.Lerp(LanternLight2.color, MagicLanternLight, Time.deltaTime * LanternTransitionSpeed);
            LanternLight2.intensity = Mathf.Lerp(LanternLight2.intensity, 0.8f, Time.deltaTime * LanternTransitionSpeed);
            LanternLight2.range = Mathf.Lerp(LanternLight2.range, 2f, Time.deltaTime * LanternTransitionSpeed);
            LanternLight3.color = Color.Lerp(LanternLight3.color, MagicLanternLight, Time.deltaTime * LanternTransitionSpeed);
            LanternLight3.intensity = Mathf.Lerp(LanternLight3.intensity, 0.8f, Time.deltaTime * LanternTransitionSpeed);
            LanternLight3.range = Mathf.Lerp(LanternLight3.range, 2f, Time.deltaTime * LanternTransitionSpeed);
            LanternLight4.color = Color.Lerp(LanternLight4.color, MagicLanternLight, Time.deltaTime * LanternTransitionSpeed);
            LanternLight4.intensity = Mathf.Lerp(LanternLight4.intensity, 0.8f, Time.deltaTime * LanternTransitionSpeed);
            LanternLight4.range = Mathf.Lerp(LanternLight4.range, 2f, Time.deltaTime * LanternTransitionSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == CompassCollider)
        {
            LanternChangeColor = true;
        }
    }
}
