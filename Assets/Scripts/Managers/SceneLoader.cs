using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Image SceneTransitionImage;
    public void LoadScene(string newScene)
    {
        SceneTransitionImage.gameObject.SetActive(true);
        SceneTransitionImage.color = new Color(0, 0, 0, 1f); // RGB + alpha: 0 transparent -> 1 opaque
        SceneManager.LoadScene(newScene);
    }
}
