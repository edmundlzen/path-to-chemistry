using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public List<TextMeshProUGUI> discoMe;
    public Gradient discoGradient;
    public float discoSpeed;
    public Image blackImage;
    public TextMeshProUGUI loadingText;
    public Image loadingBar;
    public RectTransform titleT;
    public float alphaAddAmount;
    public float titleMoveDistance;
    public Slider progressBar;
    private Task sceneLoadingTask;
    private AsyncOperation sceneLoadingOperation;
    public GameObject continueButton;

    void Start()
    {
        StartCoroutine(Disco());
        
        var directory = $"{Application.persistentDataPath}/Data";
        if (!Directory.Exists(directory))
        {
            continueButton.SetActive(false);
        }
    }

    IEnumerator Disco()
    {
        while (true)
        {
            for (float i = 0; i < 1; i += .01f)
            {
                foreach (var me in discoMe)
                {
                    me.fontSharedMaterial.SetColor("_GlowColor", discoGradient.Evaluate(i));
                }
                yield return new WaitForSeconds(discoSpeed);
            }
        }
    }

    IEnumerator TransitionToScene()
    {
        while (blackImage.color.a < 1)
        {
            float newAlpha = blackImage.color.a + alphaAddAmount;
            Color whiteColor = new Color(255, 255, 255, newAlpha);
            Color blackColor = new Color(0, 0, 0, newAlpha);
            blackImage.color = blackColor;
            loadingBar.color = whiteColor;
            loadingText.color = whiteColor;
            yield return null;
        }
        
        Vector3 desiredPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
        desiredPosition.z = titleT.transform.position.z;
        while (titleT.transform.position != desiredPosition)
        {
            titleT.transform.position = Vector3.MoveTowards(titleT.transform.position, desiredPosition, titleMoveDistance * Time.deltaTime);
            yield return null;
        }
        sceneLoadingOperation = SceneManager.LoadSceneAsync("Terrain");
        while (true) // while (sceneLoadingOperation.isDone)
        {
            titleT.transform.Rotate(0,0,titleT.transform.eulerAngles.z + .0001f);
            progressBar.value = Mathf.Clamp01(sceneLoadingOperation.progress / 0.9f);
            yield return null;
        }
    }

    public void ContinueGame()
    {
        if ((sceneLoadingTask is {Running: true})) return;
        sceneLoadingTask = new Task(TransitionToScene());
        sceneLoadingTask.Start();
    }

    public void NewGame()
    {
        if (sceneLoadingTask is {Running: true}) return;
        var directory = $"{Application.persistentDataPath}/Data";
        if (Directory.Exists(directory))
        {
            Directory.Delete(directory, true);
        }
        sceneLoadingTask = new Task(TransitionToScene());
        sceneLoadingTask.Start();
    }

    public void Quiz()
    {
        if ((sceneLoadingTask is {Running: true})) return;
        player.startPlace = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("Quiz");
    }
    
    public void Leaderboard()
    {
        if ((sceneLoadingTask is {Running: true})) return;
        player.startPlace = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("Nickname");
    }
    
    public void Lab()
    {
        if ((sceneLoadingTask is {Running: true})) return;
        player.startPlace = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("Classic Lab");
    }

    public void Credits()
    {
        if ((sceneLoadingTask is {Running: true})) return;

        SceneManager.LoadScene("Credits");
    }

    public void Quit()
    {
        if ((sceneLoadingTask is {Running: true})) return;
        
        Application.Quit();
    }
}