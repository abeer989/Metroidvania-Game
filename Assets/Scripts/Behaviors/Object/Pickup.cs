using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] protected GameObject pickupFX;
    [SerializeField] protected TMPro.TMP_Text pickupIndicatorText;

    protected virtual void OnPickup()
    {
        // detach the parent canvas from ITS parent (the pickup object) or it'll get desroyed with it:
        pickupIndicatorText.transform.parent.SetParent(null);
        pickupIndicatorText.transform.parent.position = transform.position; // but keep the transform in place (the PU object's pos.)

        // set display msg for powerup:
        pickupIndicatorText.gameObject.SetActive(true);

        if (pickupFX)
            Instantiate(pickupFX, transform.position, Quaternion.identity);

        Destroy(gameObject);
        Destroy(pickupIndicatorText.transform.parent.gameObject, 1);

        // pickup SFX:
        AudioManager.instance.PlaySFX(sfxIndex: 5);
    }
}
