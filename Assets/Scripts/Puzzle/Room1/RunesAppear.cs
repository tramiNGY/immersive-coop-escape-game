using UnityEngine;

public class RunesAppear : MonoBehaviour
{
    [SerializeField] Material runesMaterialInstance;
    public float wipeDuration = 20f;

    private float timer = 0f;
    private bool wiping = false;

    void Start()
    {
        var renderer = GetComponent<Renderer>();
        runesMaterialInstance = renderer.material; // local instance of mat to avoid alter mat prefab

        runesMaterialInstance.SetFloat("_FadeAmount", 1f); // start with invisible runes
    }

    void Update()
    {
        if (wiping)
        {
            timer += Time.deltaTime; // adds time since last frame, so fading progress overtime
            float fade = Mathf.Clamp01(timer / wipeDuration); // calculates how much time/total fade time, Clamp01 keeps value between 0 and 1
            runesMaterialInstance.SetFloat("_FadeAmount", 1f - fade); // fade from 1(invisible) to 0 (visible), fade increase each frame closer to 1

            if (fade >= 1f) // when fade reaches one and FadeAmount is at 0 (visible)
                wiping = false;  // condition stop update fade
        }
    }

    // Launch runes fade
    public void StartFade()
    {
        Debug.Log("StartFade called");
        timer = 0f;
        wiping = true; // condition to start fading
    }

    public void OnEnable() // called when gameObject is SetActive(true)
    {
        StartFade();
    }
}
