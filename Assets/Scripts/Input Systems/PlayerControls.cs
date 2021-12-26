// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input Systems/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""d213f9eb-c238-4cd4-b30c-8385d7516685"",
            ""actions"": [
                {
                    ""name"": ""HorizontalMovement"",
                    ""type"": ""Value"",
                    ""id"": ""b1a9ac87-b0ce-419c-8ab0-aa7e56a2bfb7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""VerticalMovement"",
                    ""type"": ""Value"",
                    ""id"": ""5b836b16-e8c3-44d4-b8da-c735cec93474"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraMovement"",
                    ""type"": ""Value"",
                    ""id"": ""3640874a-5388-4de7-ab20-4db7e457800d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraRollMod"",
                    ""type"": ""Value"",
                    ""id"": ""2411eed9-000d-4dc4-a650-ca7ab8733d15"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Grip"",
                    ""type"": ""Button"",
                    ""id"": ""24498500-1d23-4daa-8812-46755aa39c9b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""88fe6a91-cdbc-41c2-9387-6e0076ad15c1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""72769cf9-c0b2-4e02-8e40-93b3ef98f8d7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ability"",
                    ""type"": ""Button"",
                    ""id"": ""1bc5efd8-3adf-4b7d-9fdb-ab24e00f7309"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EquipWeapon1"",
                    ""type"": ""Button"",
                    ""id"": ""c1f2a476-0e1e-4fe5-b906-4620bfa759e5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EquipWeapon2"",
                    ""type"": ""Button"",
                    ""id"": ""53882fc7-1d0b-4167-8670-d892594aab66"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""43ae6a52-371b-4499-abfa-887470fb41c1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ZoomIn"",
                    ""type"": ""Button"",
                    ""id"": ""a8616ad7-8d71-4ce6-b405-3786f55c6668"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""5cb844d6-34f6-41cb-8876-2e20054d42c5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""995a7d87-6453-4770-8710-754de289df21"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""912e418a-4981-42f7-9de1-144333727701"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e14f24a7-7dc8-4ceb-a182-fbde1bd89467"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b1be1984-9be0-4ebe-b306-7264aaf61afc"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""1740ffb7-db96-4483-aa36-71c1a7af7af8"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b0c92eb9-5d3c-45e6-8215-920ecf0985a2"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0cc95074-ebc6-47db-a8e1-347c1f81674a"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2f070f5a-0e6f-4ee5-8c95-25487918b459"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c0f7cb91-bfc9-47b0-a676-68098918a084"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRollMod"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a1ea010-46ec-4742-94d4-729443cbec0f"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3513d883-c664-4184-ae11-08acc4ccc346"",
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
                    ""id"": ""1983e932-72e3-42c9-884a-95be1d6b6a03"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e2a77762-6a49-44ed-94e9-a9b9413fd6b2"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ability"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19837e90-6714-4144-b972-9c23f8d9b255"",
                    ""path"": ""<Mouse>/forwardButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ability"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f41328dc-8974-4978-b761-40dece3bda54"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipWeapon1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""70322399-4bd8-4d79-ba68-7042332f46f1"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipWeapon2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8e36e409-d6eb-47fb-87a2-42dc66bdb916"",
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
                    ""id"": ""684469d2-6172-4dc5-b49b-849c2496826f"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomIn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_HorizontalMovement = m_Player.FindAction("HorizontalMovement", throwIfNotFound: true);
        m_Player_VerticalMovement = m_Player.FindAction("VerticalMovement", throwIfNotFound: true);
        m_Player_CameraMovement = m_Player.FindAction("CameraMovement", throwIfNotFound: true);
        m_Player_CameraRollMod = m_Player.FindAction("CameraRollMod", throwIfNotFound: true);
        m_Player_Grip = m_Player.FindAction("Grip", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Fire = m_Player.FindAction("Fire", throwIfNotFound: true);
        m_Player_Ability = m_Player.FindAction("Ability", throwIfNotFound: true);
        m_Player_EquipWeapon1 = m_Player.FindAction("EquipWeapon1", throwIfNotFound: true);
        m_Player_EquipWeapon2 = m_Player.FindAction("EquipWeapon2", throwIfNotFound: true);
        m_Player_Reload = m_Player.FindAction("Reload", throwIfNotFound: true);
        m_Player_ZoomIn = m_Player.FindAction("ZoomIn", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_HorizontalMovement;
    private readonly InputAction m_Player_VerticalMovement;
    private readonly InputAction m_Player_CameraMovement;
    private readonly InputAction m_Player_CameraRollMod;
    private readonly InputAction m_Player_Grip;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Fire;
    private readonly InputAction m_Player_Ability;
    private readonly InputAction m_Player_EquipWeapon1;
    private readonly InputAction m_Player_EquipWeapon2;
    private readonly InputAction m_Player_Reload;
    private readonly InputAction m_Player_ZoomIn;
    public struct PlayerActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @HorizontalMovement => m_Wrapper.m_Player_HorizontalMovement;
        public InputAction @VerticalMovement => m_Wrapper.m_Player_VerticalMovement;
        public InputAction @CameraMovement => m_Wrapper.m_Player_CameraMovement;
        public InputAction @CameraRollMod => m_Wrapper.m_Player_CameraRollMod;
        public InputAction @Grip => m_Wrapper.m_Player_Grip;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Fire => m_Wrapper.m_Player_Fire;
        public InputAction @Ability => m_Wrapper.m_Player_Ability;
        public InputAction @EquipWeapon1 => m_Wrapper.m_Player_EquipWeapon1;
        public InputAction @EquipWeapon2 => m_Wrapper.m_Player_EquipWeapon2;
        public InputAction @Reload => m_Wrapper.m_Player_Reload;
        public InputAction @ZoomIn => m_Wrapper.m_Player_ZoomIn;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @HorizontalMovement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHorizontalMovement;
                @HorizontalMovement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHorizontalMovement;
                @HorizontalMovement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHorizontalMovement;
                @VerticalMovement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnVerticalMovement;
                @VerticalMovement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnVerticalMovement;
                @VerticalMovement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnVerticalMovement;
                @CameraMovement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraMovement;
                @CameraMovement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraMovement;
                @CameraMovement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraMovement;
                @CameraRollMod.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraRollMod;
                @CameraRollMod.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraRollMod;
                @CameraRollMod.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraRollMod;
                @Grip.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGrip;
                @Grip.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGrip;
                @Grip.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGrip;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Fire.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                @Ability.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbility;
                @Ability.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbility;
                @Ability.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAbility;
                @EquipWeapon1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipWeapon1;
                @EquipWeapon1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipWeapon1;
                @EquipWeapon1.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipWeapon1;
                @EquipWeapon2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipWeapon2;
                @EquipWeapon2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipWeapon2;
                @EquipWeapon2.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipWeapon2;
                @Reload.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnReload;
                @ZoomIn.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZoomIn;
                @ZoomIn.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZoomIn;
                @ZoomIn.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnZoomIn;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HorizontalMovement.started += instance.OnHorizontalMovement;
                @HorizontalMovement.performed += instance.OnHorizontalMovement;
                @HorizontalMovement.canceled += instance.OnHorizontalMovement;
                @VerticalMovement.started += instance.OnVerticalMovement;
                @VerticalMovement.performed += instance.OnVerticalMovement;
                @VerticalMovement.canceled += instance.OnVerticalMovement;
                @CameraMovement.started += instance.OnCameraMovement;
                @CameraMovement.performed += instance.OnCameraMovement;
                @CameraMovement.canceled += instance.OnCameraMovement;
                @CameraRollMod.started += instance.OnCameraRollMod;
                @CameraRollMod.performed += instance.OnCameraRollMod;
                @CameraRollMod.canceled += instance.OnCameraRollMod;
                @Grip.started += instance.OnGrip;
                @Grip.performed += instance.OnGrip;
                @Grip.canceled += instance.OnGrip;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Ability.started += instance.OnAbility;
                @Ability.performed += instance.OnAbility;
                @Ability.canceled += instance.OnAbility;
                @EquipWeapon1.started += instance.OnEquipWeapon1;
                @EquipWeapon1.performed += instance.OnEquipWeapon1;
                @EquipWeapon1.canceled += instance.OnEquipWeapon1;
                @EquipWeapon2.started += instance.OnEquipWeapon2;
                @EquipWeapon2.performed += instance.OnEquipWeapon2;
                @EquipWeapon2.canceled += instance.OnEquipWeapon2;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @ZoomIn.started += instance.OnZoomIn;
                @ZoomIn.performed += instance.OnZoomIn;
                @ZoomIn.canceled += instance.OnZoomIn;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnHorizontalMovement(InputAction.CallbackContext context);
        void OnVerticalMovement(InputAction.CallbackContext context);
        void OnCameraMovement(InputAction.CallbackContext context);
        void OnCameraRollMod(InputAction.CallbackContext context);
        void OnGrip(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnAbility(InputAction.CallbackContext context);
        void OnEquipWeapon1(InputAction.CallbackContext context);
        void OnEquipWeapon2(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnZoomIn(InputAction.CallbackContext context);
    }
}
