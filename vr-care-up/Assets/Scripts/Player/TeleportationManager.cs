using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    public GameObject teleportationGameObject;
    public InputActionReference teleportActivationReference;
    PlayerScript player;

    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCancel;
    public XRRayInteractor xrRayInteractor;
    public ActionBasedController actionBasedController;
        
    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScript>();
        teleportActivationReference.action.performed += TeleportModeActivate;
        teleportActivationReference.action.canceled += TeleportModeCancel;
    }

    private void TeleportModeActivate(InputAction.CallbackContext obj) => onTeleportActivate.Invoke();

    private void TeleportModeCancel(InputAction.CallbackContext obj) => Invoke("DeactivateTeleporter", 0.1f);

    void DeactivateTeleporter() => onTeleportCancel.Invoke();

    public void TeleportModeActivation()
    {
        if (!player.IsInActionNoTimeout())
        {
            actionBasedController.enableInputActions = true;
            xrRayInteractor.enabled = true;
        }
    }
}
