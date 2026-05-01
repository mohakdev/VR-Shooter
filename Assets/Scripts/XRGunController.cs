using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGunController : MonoBehaviour
{
    private Gun currentGun;
    private ITool currentTool;

    [SerializeField] TextMeshProUGUI gunText;

    private XRBaseInteractor interactor;
    private ActionBasedController controller;

    public InputActionProperty reloadAction;

    void Awake()
    {
        interactor = GetComponent<XRBaseInteractor>();
        controller = GetComponent<ActionBasedController>();

        interactor.selectEntered.AddListener(OnGrab);
        interactor.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        Transform obj = args.interactableObject.transform;

        currentGun = obj.GetComponent<Gun>();
        currentTool = obj.GetComponent<ITool>();

        gunText.text = obj.name;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        currentGun = null;
        currentTool = null;
        gunText.text = "";
    }

    void Update()
    {
        if (controller == null) return;

        var activate = controller.activateAction.action;

        bool isHeld = activate.IsPressed();
        bool pressedThisFrame = activate.WasPressedThisFrame();

        if (currentGun != null)
        {
            if (currentGun.isAutomatic)
            {
                if (isHeld)
                    currentGun.TryShoot();
            }
            else
            {
                if (pressedThisFrame)
                    currentGun.TryShoot();
            }

            HandleReload();
        }
        if (currentTool != null)
        {
            currentTool.OnPrimaryAction(isHeld, pressedThisFrame);
        }
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