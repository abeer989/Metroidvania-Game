using ScriptableEvents.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int newGameSceneIndex;
    [SerializeField] GameObject continueButton;
    //[SerializeField] PlayerAbilityTracker playerAbilityTracker;

    [Title("Scriptable Events")]
    [SerializeField] IntScriptableEvent musicEvent;

    void Start()
    {
        musicEvent.Raise(0); // play main menu music

        if (PlayerPrefs.HasKey("continue_level_index"))
            continueButton.SetActive(true);
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll(); // starting a new game will delete all player progress (TODO: add a warning prompt)
        SceneManager.LoadScene(newGameSceneIndex);
    }

    public void ContinueGame()
    {
        int loadSceneIndex = PlayerPrefs.GetInt("continue_level_index", 1);
        string loadPositionString = PlayerPrefs.GetString("continue_position");

        // loading scene & position:
        SceneManager.LoadScene(loadSceneIndex);

        Vector3 loadPosition = Vector3.zero;

        if (!string.IsNullOrEmpty(loadPositionString))
            loadPosition = StringToVector3(loadPositionString);

        playerAbilityTracker.gameObject.SetActive(true);
        playerAbilityTracker.transform.position = loadPosition;

        #region Old Abililty Loading (Using PlayerPrefs)
        // loading unlocked abilities:
        //if (PlayerPrefs.HasKey("double_jump_unlocked") && PlayerPrefs.GetInt("double_jump_unlocked") == 1)
        //    player.doubleJumpUnlocked = true;        

        //if (PlayerPrefs.HasKey("dash_unlocked") && PlayerPrefs.GetInt("dash_unlocked") == 1)
        //    player.dashUnlocked = true;        

        //if (PlayerPrefs.HasKey("ball_mode_unlocked") && PlayerPrefs.GetInt("ball_mode_unlocked") == 1)
        //    player.ballModeUnlocked = true;        

        //if (PlayerPrefs.HasKey("drop_bombs_unlocked") && PlayerPrefs.GetInt("drop_bombs_unlocked") == 1)
        //    player.dropBombsUnlocked = true; 
        #endregion
    }

    public void QuitGame() => Application.Quit();

    private Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            sVector = sVector.Substring(1, sVector.Length - 2);

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}
