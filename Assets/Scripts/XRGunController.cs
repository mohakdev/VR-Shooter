using RadiantTools.AudioSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGunController : MonoBehaviour
{
    private Gun currentGun;
    [SerializeField] TextMeshProUGUI gunText;

    private XRBaseInteractor interactor;
    private ActionBasedController controller;
    public InputActionProperty reloadAction;

    void Awake()
    {
        // Get both systems
        interactor = GetComponent<XRBaseInteractor>();
        controller = GetComponent<ActionBasedController>();

        // Listen for grab events
        interactor.selectEntered.AddListener(OnGrab);
        interactor.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        currentGun = args.interactableObject.transform.GetComponent<Gun>();
        gunText.text = currentGun.name;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        currentGun = null;
        gunText.text = "";
    }

    void Update()
    {
        if (currentGun == null || controller == null) return;

        var activate = controller.activateAction.action;

        if (currentGun.isAutomatic)
        {
            if (activate.IsPressed())
                currentGun.TryShoot();
        }
        else
        {
            if (activate.WasPressedThisFrame())
                currentGun.TryShoot();
        }
        HandleReload();
    }
    void HandleReload()
    {
        var reload = reloadAction.action;

        if (reload != null && reload.WasPressedThisFrame())
        {
            currentGun.Reload();
        }
    }
}