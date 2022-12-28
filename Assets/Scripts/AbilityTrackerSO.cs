using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTrackerSO", menuName = "Abilities/Create Ability Tracker SO")]
public class AbilityTrackerSO : ScriptableObject
{
    [SerializeField] bool doubleJumpUnlocked;
    [SerializeField] bool dashUnlocked;
    [SerializeField] bool ballModeUnlocked;
    [SerializeField] bool dropBombsUnlocked;

    public void UnlockAll()
    {
        doubleJumpUnlocked = true;
        dashUnlocked = true;
        ballModeUnlocked = true;
        dropBombsUnlocked = true;
    }

    public bool DoubleJumpUnlocked
    {
        get => doubleJumpUnlocked;
        set => doubleJumpUnlocked = value;
    }    
    
    public bool DashUnlocked
    {
        get => dashUnlocked;
        set => dashUnlocked = value;
    }    
    
    public bool BallModeUnlocked
    {
        get => ballModeUnlocked;
        set => ballModeUnlocked = value;
    }    
    
    public bool DropBombsUnlocked
    {
        get => dropBombsUnlocked;
        set => dropBombsUnlocked = value;
    }
}