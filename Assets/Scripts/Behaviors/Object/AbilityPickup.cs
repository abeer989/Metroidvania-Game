using UnityEngine;
using Sirenix.OdinInspector;

public class AbilityPickup : Pickup
{
    [SerializeField] AbilityType abilityType;

    [Title("Scriptable Objects")]
    [SerializeField] AbilityTrackerSO abilityTrackerSO;

    public enum AbilityType
    {
        none,
        all,
        double_jump,
        dash,
        ball_mode,
        drop_bombs
    }

    protected override void OnPickup()
    {
        base.OnPickup();

        switch (abilityType)
        {
            case AbilityType.double_jump:
                abilityTrackerSO.DoubleJumpUnlocked = true;
                pickupIndicatorText.SetText("DOUBLE JUMP UNLOCKED!");
                //PlayerPrefs.SetInt("double_jump_unlocked", 1);
                break;

            case AbilityType.dash:
                abilityTrackerSO.DashUnlocked = true;
                pickupIndicatorText.SetText("DASH UNLOCKED!");
                //PlayerPrefs.SetInt("dash_unlocked", 1);
                break;

            case AbilityType.ball_mode:
                abilityTrackerSO.BallModeUnlocked = true;
                pickupIndicatorText.SetText("BALL MODE UNLOCKED!");
                //PlayerPrefs.SetInt("ball_mode_unlocked", 1);
                break;

            case AbilityType.drop_bombs:
                abilityTrackerSO.DropBombsUnlocked = true;
                pickupIndicatorText.SetText("BOMB DROP UNLOCKED!");
                //PlayerPrefs.SetInt("drop_bombs_unlocked", 1);
                break;

            case AbilityType.all:
                abilityTrackerSO.UnlockAll();
                pickupIndicatorText.SetText("ALL ABILITIES UNLOCKED!");
                break;

            default:
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            OnPickup();
    }
}
