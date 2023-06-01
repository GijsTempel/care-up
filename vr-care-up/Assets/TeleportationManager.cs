using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class TeleportationManager : MonoBehaviour
{
    public GameObject baseControllerGameObject;
    public GameObject teleportationGameObject;
    public InputActionReference teleportActivationReference;

    [Space]
    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCancel;

    private void Start()
    {
        teleportActivationReference.action.performed += TeleportModeActivate;
        teleportActivationReference.action.canceled += TeleportModeCancel;
    }

    private void TeleportModeActivate(InputAction.CallbackContext obj) => onTeleportActivate.Invoke();

    private void TeleportModeCancel(InputAction.CallbackContext obj) => Invoke("DeactivateTeleporter", 0.1f);

    void DeactivateTeleporter() => onTeleportCancel.Invoke();

}
