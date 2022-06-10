using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] Slider healthBar;
    [SerializeField] Image fadeScreen;
    [SerializeField] TMPro.TMP_Text checkpointText;
    
    [SerializeField] float fadeSpeed;

    bool fadingToBlack;
    bool fadingFromBlack;

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
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
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
