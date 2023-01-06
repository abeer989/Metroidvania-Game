using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Persistent Game Data", menuName = "Game Data/Create Persistent Game Data")]
public class GamePersistentDataSO : ScriptableObject
{
    [BoxGroup("Game State")] [SerializeField] private bool continueGame;
    
    [BoxGroup("Player State")] [SerializeField] private int lastSceneIndex;
    [BoxGroup("Player State")] [SerializeField] private Vector3 lastPlayerPos;

    public bool ContinueGame
    {
        get => continueGame;
        set => continueGame = value;
    }    
    
    public int LastSceneIndex
    {
        get => lastSceneIndex;
        set => lastSceneIndex = value;
    }   
    
    public Vector3 LastPlayerPos
    {
        get => lastPlayerPos;
        set => lastPlayerPos = value;
    }


}
