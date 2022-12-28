using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public bool isGamePaused;

    [SerializeField] int mainMenuSceneIndex;

    [Space]
    [SerializeField] GameObject pauseScreen;
    public GameObject fullScreenMap;
    public GameObject miniMap;

    [Space]
    [SerializeField] Slider healthBar;
    [SerializeField] Image fadeScreen;
    [SerializeField] TMPro.TMP_Text checkpointText;

    [Space]
    [SerializeField] float fadeSpeed;

    bool fadingToBlack;
    bool fadingFromBlack;

    public GameObject MiniMap
    {
        get { return miniMap; }
    }

    public GameObject FullscreenMap
    {
        get { return fullScreenMap; }
    }

    private void Awake()
    {
        // Singleton:
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);

        Time.timeScale = 1;
    }

    private void Update()
    {
        // if the fadingToBlack is on, the fade image's alpha value will gradually move from 0 -> 1:
        if (fadingToBlack)
        {
            fadeScreen.color = new Color(r: fadeScreen.color.r,
                                         g: fadeScreen.color.g,
                                         b: fadeScreen.color.b,
                                         a: Mathf.MoveTowards(fadeScreen.color.a, 1, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 1)
                fadingToBlack = false;
        }

        // and vice versa if the fadingFromBlack bool is on
        else if (fadingFromBlack)
        {
            fadeScreen.color = new Color(r: fadeScreen.color.r,
                                         g: fadeScreen.color.g,
                                         b: fadeScreen.color.b,
                                         a: Mathf.MoveTowards(fadeScreen.color.a, 0, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 0)
                fadingFromBlack = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseUnpause();
    }

    public void MainMenu()
    {
        Destroy(PlayerHealthController.instance.gameObject);
        PlayerHealthController.instance = null;

        Destroy(RespawnController.instance.gameObject);
        RespawnController.instance = null;

        Destroy(MapController.instance.gameObject);
        MapController.instance = null;

        Destroy(gameObject);
        instance = null;

        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void PauseUnpause()
    {
        if (!pauseScreen.activeSelf)
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
            isGamePaused = true;
        }

        else
        {
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
            isGamePaused = false;
        }
    }

    public void UpdateHealth(float currentHealth/*, float maxHealth*/)
    {
        //healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void UpdateMaxHealth(float maxHealth)
    {
        healthBar.maxValue = maxHealth;
    }

    // Set bool values via thse functions that will determine the fading in/out:
    public void SetFadeToBlack()
    {
        fadingToBlack = true;
        fadingFromBlack = false;
    }

    public void SetFadeFromBlack()
    {
        fadingFromBlack = true;
        fadingToBlack = false;
    }

    public void CallShowCheckpointTextCR() => StartCoroutine(ShowCheckpointTextCR());

    IEnumerator ShowCheckpointTextCR()
    {
        checkpointText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        checkpointText.gameObject.SetActive(false);
    }
}