using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// https://gist.github.com/NovaSurfer/5f14e9153e7a2a07d7c5
/// </summary>
public class ScreenFader : MonoBehaviour {

    public Image FadeImg;
    public float FadeSpeed = 1.5f;
    public bool SceneStarting = true;


    private void Awake()
    {
        FadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        // If the scene is starting...
        if (SceneStarting)
            // ... call the StartScene function.
            StartScene();
    }


    private void FadeToClear()
    {
        // Lerp the colour of the image between itself and transparent.
        FadeImg.color = Color.Lerp(FadeImg.color, Color.clear, FadeSpeed * Time.deltaTime);
    }


    private void FadeToBlack()
    {
        // Lerp the colour of the image between itself and black.
        FadeImg.color = Color.Lerp(FadeImg.color, Color.black, FadeSpeed * Time.deltaTime);
    }


    private void StartScene()
    {
        // Fade the texture to clear.
        FadeToClear();

        // If the texture is almost clear...
        if (FadeImg.color.a <= 0.05f)
        {
            // ... set the colour to clear and disable the RawImage.
            FadeImg.color = Color.clear;
            FadeImg.enabled = false;

            // The scene is no longer starting.
            SceneStarting = false;
        }
    }


    public IEnumerator EndSceneRoutine(int sceneNumber)
    {
        // Make sure the RawImage is enabled.
        FadeImg.enabled = true;
        do
        {
            // Start fading towards black.
            FadeToBlack();

            // If the screen is almost black...
            if (FadeImg.color.a >= 0.95f)
            {
                // ... reload the level
                SceneManager.LoadScene(sceneNumber);
                yield break;
            }
            else
            {
                yield return null;
            }
        } while (true);
    }

    public void EndScene(int sceneNumber)
    {
        SceneStarting = false;
        StartCoroutine("EndSceneRoutine", sceneNumber);
    }
}