// GENERATED AUTOMATICALLY FROM 'Assets/GameInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""Master"",
            ""id"": ""82ccc3bf-bead-4759-b5a6-5e21fc82d68d"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""6065d527-9292-4e78-9d17-ef3d4c82f865"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""bb26d88d-d815-4b6b-b557-d13ccece4a0b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Primary"",
                    ""type"": ""Button"",
                    ""id"": ""eaa7710b-2dd4-47c4-8044-635c52cdb489"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Secondary"",
                    ""type"": ""Button"",
                    ""id"": ""9a83a3b0-eb3e-4415-a66f-698245da7e15"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Value"",
                    ""id"": ""816f6c1c-342e-493e-bc29-66056a10ac86"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""f8da7f24-7b78-4c28-9192-ef657fb96194"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""3b91ea1c-953d-42e4-8988-fcf2ab8b4ef5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""96f3034b-79fe-4421-b37b-99b3cba53a58"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""e18c0852-8235-4ae4-baa8-14ef31203f87"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Flashlight"",
                    ""type"": ""Button"",
                    ""id"": ""43a735fd-9709-4624-b4bc-b2e3c8783493"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Grenade"",
                    ""type"": ""Button"",
                    ""id"": ""8bd7e29d-c9e4-4bc7-8925-1ee7031b7760"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Drop"",
                    ""type"": ""Button"",
                    ""id"": ""8dffd577-06dc-44e9-bf53-67f8a9a0527b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WSAD"",
                    ""id"": ""d00f09ed-6048-464c-bf22-8911340c4b41"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6125b25b-891c-4530-ab82-601cf6a7bcd7"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB+M"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b7b0ed0b-5de9-44e9-a74e-3e67c6d367b3"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB+M"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""aa3dfeb9-2e8c-42ff-9d47-d933e44211c6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB+M"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e0f8fcae-f6a8-4886-ae3e-ae18e15f50b2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB+M"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0b29abc5-c33c-41c1-87ab-919cec755643"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB+M"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c561808b-d960-4c5f-8925-c18a2cd0fc1e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB+M"",
                    ""action"": ""Primary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8aabd4fd-52da-44e7-b7a8-70c8f8ba1547"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB+M"",
                    ""action"": ""Secondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f332558-8a89-4184-ae6b-af3e367139ae"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB+M"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1744f96e-8e5f-4d8e-a9ba-0769f076e1ad"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB+M"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3be6995a-6a73-4859-a570-ed7c6ccd0960"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KB+M"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da8d236d-a28f-45e1-ad9b-9652a522d2d1"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c5e17a5-95ab-4fcc-98b3-a02301eb9c5d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8af44ac-d3d5-4841-ad62-91482d5e0166"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flashlight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd057f82-b349-424a-adac-7855748ea763"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grenade"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6e007bb-4830-48d7-9a35-2d8f70c1b177"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Settings"",
            ""id"": ""06f22f79-449b-45e6-87b6-9dd3a55cbe28"",
            ""actions"": [
                {
                    ""name"": ""Lock"",
                    ""type"": ""Button"",
                    ""id"": ""f2131560-9338-4fd2-8a62-d537e490dd90"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Unlock"",
                    ""type"": ""Button"",
                    ""id"": ""e2e7e621-833d-423f-bb1c-c913f53ae1fd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Scoreboard"",
                    ""type"": ""Button"",
                    ""id"": ""2755feda-0a35-4be4-9e6d-b1d5ded44a00"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""08ff8a9e-da02-4bca-bb15-8f176db784a4"",
                    ""path"": ""<Keyboard>/f1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7b4a38ed-bc07-42fb-96b4-23ade3aeb91e"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Unlock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a1497f3-eb97-4b9d-8a60-8d3a01c144b2"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scoreboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KB+M"",
            ""bindingGroup"": ""KB+M"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Master
        m_Master = asset.FindActionMap("Master", throwIfNotFound: true);
        m_Master_Movement = m_Master.FindAction("Movement", throwIfNotFound: true);
        m_Master_Look = m_Master.FindAction("Look", throwIfNotFound: true);
        m_Master_Primary = m_Master.FindAction("Primary", throwIfNotFound: true);
        m_Master_Secondary = m_Master.FindAction("Secondary", throwIfNotFound: true);
        m_Master_Select = m_Master.FindAction("Select", throwIfNotFound: true);
        m_Master_Interact = m_Master.FindAction("Interact", throwIfNotFound: true);
        m_Master_Crouch = m_Master.FindAction("Crouch", throwIfNotFound: true);
        m_Master_Reload = m_Master.FindAction("Reload", throwIfNotFound: true);
        m_Master_Jump = m_Master.FindAction("Jump", throwIfNotFound: true);
        m_Master_Flashlight = m_Master.FindAction("Flashlight", throwIfNotFound: true);
        m_Master_Grenade = m_Master.FindAction("Grenade", throwIfNotFound: true);
        m_Master_Drop = m_Master.FindAction("Drop", throwIfNotFound: true);
        // Settings
        m_Settings = asset.FindActionMap("Settings", throwIfNotFound: true);
        m_Settings_Lock = m_Settings.FindAction("Lock", throwIfNotFound: true);
        m_Settings_Unlock = m_Settings.FindAction("Unlock", throwIfNotFound: true);
        m_Settings_Scoreboard = m_Settings.FindAction("Scoreboard", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Master
    private readonly InputActionMap m_Master;
    private IMasterActions m_MasterActionsCallbackInterface;
    private readonly InputAction m_Master_Movement;
    private readonly InputAction m_Master_Look;
    private readonly InputAction m_Master_Primary;
    private readonly InputAction m_Master_Secondary;
    private readonly InputAction m_Master_Select;
    private readonly InputAction m_Master_Interact;
    private readonly InputAction m_Master_Crouch;
    private readonly InputAction m_Master_Reload;
    private readonly InputAction m_Master_Jump;
    private readonly InputAction m_Master_Flashlight;
    private readonly InputAction m_Master_Grenade;
    private readonly InputAction m_Master_Drop;
    public struct MasterActions
    {
        private @GameInput m_Wrapper;
        public MasterActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Master_Movement;
        public InputAction @Look => m_Wrapper.m_Master_Look;
        public InputAction @Primary => m_Wrapper.m_Master_Primary;
        public InputAction @Secondary => m_Wrapper.m_Master_Secondary;
        public InputAction @Select => m_Wrapper.m_Master_Select;
        public InputAction @Interact => m_Wrapper.m_Master_Interact;
        public InputAction @Crouch => m_Wrapper.m_Master_Crouch;
        public InputAction @Reload => m_Wrapper.m_Master_Reload;
        public InputAction @Jump => m_Wrapper.m_Master_Jump;
        public InputAction @Flashlight => m_Wrapper.m_Master_Flashlight;
        public InputAction @Grenade => m_Wrapper.m_Master_Grenade;
        public InputAction @Drop => m_Wrapper.m_Master_Drop;
        public InputActionMap Get() { return m_Wrapper.m_Master; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MasterActions set) { return set.Get(); }
        public void SetCallbacks(IMasterActions instance)
        {
            if (m_Wrapper.m_MasterActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnMovement;
                @Look.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnLook;
                @Primary.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnPrimary;
                @Primary.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnPrimary;
                @Primary.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnPrimary;
                @Secondary.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnSecondary;
                @Secondary.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnSecondary;
                @Secondary.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnSecondary;
                @Select.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnSelect;
                @Interact.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnInteract;
                @Crouch.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnCrouch;
                @Reload.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnReload;
                @Jump.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnJump;
                @Flashlight.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnFlashlight;
                @Flashlight.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnFlashlight;
                @Flashlight.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnFlashlight;
                @Grenade.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnGrenade;
                @Grenade.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnGrenade;
                @Grenade.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnGrenade;
                @Drop.started -= m_Wrapper.m_MasterActionsCallbackInterface.OnDrop;
                @Drop.performed -= m_Wrapper.m_MasterActionsCallbackInterface.OnDrop;
                @Drop.canceled -= m_Wrapper.m_MasterActionsCallbackInterface.OnDrop;
            }
            m_Wrapper.m_MasterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Primary.started += instance.OnPrimary;
                @Primary.performed += instance.OnPrimary;
                @Primary.canceled += instance.OnPrimary;
                @Secondary.started += instance.OnSecondary;
                @Secondary.performed += instance.OnSecondary;
                @Secondary.canceled += instance.OnSecondary;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Flashlight.started += instance.OnFlashlight;
                @Flashlight.performed += instance.OnFlashlight;
                @Flashlight.canceled += instance.OnFlashlight;
                @Grenade.started += instance.OnGrenade;
                @Grenade.performed += instance.OnGrenade;
                @Grenade.canceled += instance.OnGrenade;
                @Drop.started += instance.OnDrop;
                @Drop.performed += instance.OnDrop;
                @Drop.canceled += instance.OnDrop;
            }
        }
    }
    public MasterActions @Master => new MasterActions(this);

    // Settings
    private readonly InputActionMap m_Settings;
    private ISettingsActions m_SettingsActionsCallbackInterface;
    private readonly InputAction m_Settings_Lock;
    private readonly InputAction m_Settings_Unlock;
    private readonly InputAction m_Settings_Scoreboard;
    public struct SettingsActions
    {
        private @GameInput m_Wrapper;
        public SettingsActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Lock => m_Wrapper.m_Settings_Lock;
        public InputAction @Unlock => m_Wrapper.m_Settings_Unlock;
        public InputAction @Scoreboard => m_Wrapper.m_Settings_Scoreboard;
        public InputActionMap Get() { return m_Wrapper.m_Settings; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SettingsActions set) { return set.Get(); }
        public void SetCallbacks(ISettingsActions instance)
        {
            if (m_Wrapper.m_SettingsActionsCallbackInterface != null)
            {
                @Lock.started -= m_Wrapper.m_SettingsActionsCallbackInterface.OnLock;
                @Lock.performed -= m_Wrapper.m_SettingsActionsCallbackInterface.OnLock;
                @Lock.canceled -= m_Wrapper.m_SettingsActionsCallbackInterface.OnLock;
                @Unlock.started -= m_Wrapper.m_SettingsActionsCallbackInterface.OnUnlock;
                @Unlock.performed -= m_Wrapper.m_SettingsActionsCallbackInterface.OnUnlock;
                @Unlock.canceled -= m_Wrapper.m_SettingsActionsCallbackInterface.OnUnlock;
                @Scoreboard.started -= m_Wrapper.m_SettingsActionsCallbackInterface.OnScoreboard;
                @Scoreboard.performed -= m_Wrapper.m_SettingsActionsCallbackInterface.OnScoreboard;
                @Scoreboard.canceled -= m_Wrapper.m_SettingsActionsCallbackInterface.OnScoreboard;
            }
            m_Wrapper.m_SettingsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Lock.started += instance.OnLock;
                @Lock.performed += instance.OnLock;
                @Lock.canceled += instance.OnLock;
                @Unlock.started += instance.OnUnlock;
                @Unlock.performed += instance.OnUnlock;
                @Unlock.canceled += instance.OnUnlock;
                @Scoreboard.started += instance.OnScoreboard;
                @Scoreboard.performed += instance.OnScoreboard;
                @Scoreboard.canceled += instance.OnScoreboard;
            }
        }
    }
    public SettingsActions @Settings => new SettingsActions(this);
    private int m_KBMSchemeIndex = -1;
    public InputControlScheme KBMScheme
    {
        get
        {
            if (m_KBMSchemeIndex == -1) m_KBMSchemeIndex = asset.FindControlSchemeIndex("KB+M");
            return asset.controlSchemes[m_KBMSchemeIndex];
        }
    }
    public interface IMasterActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnPrimary(InputAction.CallbackContext context);
        void OnSecondary(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnFlashlight(InputAction.CallbackContext context);
        void OnGrenade(InputAction.CallbackContext context);
        void OnDrop(InputAction.CallbackContext context);
    }
    public interface ISettingsActions
    {
        void OnLock(InputAction.CallbackContext context);
        void OnUnlock(InputAction.CallbackContext context);
        void OnScoreboard(InputAction.CallbackContext context);
    }
}
