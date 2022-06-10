using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int newGameSceneIndex;

    void Start()
    {
        
    }

    public void NewGame() => SceneManager.LoadScene(newGameSceneIndex);

    public void QuitGame() => Application.Quit();
}
