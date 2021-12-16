//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Inputs/Inputs.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputTank : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputTank()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputs"",
    ""maps"": [
        {
            ""name"": ""Tank"",
            ""id"": ""bfa9f3e3-921f-4716-88e3-9109f2e335dc"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""084daaa5-2c35-4250-aeb2-d83301ee7b33"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Value"",
                    ""id"": ""3f193075-7b2c-45c7-b6d1-7fbaf70f3ec9"",
                    ""expectedControlType"": ""Touch"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MouseMove"",
                    ""type"": ""Value"",
                    ""id"": ""f179d1e8-3822-44b8-9ed3-3bcac4ba6852"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""FireGameStick"",
                    ""type"": ""Value"",
                    ""id"": ""e639a998-636f-4170-82dc-889aca9f36e3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""95af7a20-f560-42b8-8c77-d4fb5b0c2dfb"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""a088a7de-207f-478c-86a2-6c366240222a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""20aab67b-c8ac-4eb9-8108-578d992b66bd"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""36785a0c-753d-40f2-9325-cc54c2d6681e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""166b3118-245b-4a7d-adc0-ec9b220c66d5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6bad2d3e-7b66-432d-b258-f821c28ca702"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4958c32e-38de-4c21-9936-c46b6a7d4c75"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MobileScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""5b2b379e-ab24-4cb3-8bf4-95e90d3d974a"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08842d46-d51b-4dde-b0a8-b04675a980b8"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""MouseMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ad7d3ae-6eb6-406d-a833-9ea80372fb85"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MobileScheme"",
                    ""action"": ""FireGameStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardMouse"",
            ""bindingGroup"": ""KeyboardMouse"",
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
        },
        {
            ""name"": ""MobileScheme"",
            ""bindingGroup"": ""MobileScheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Tank
        m_Tank = asset.FindActionMap("Tank", throwIfNotFound: true);
        m_Tank_Move = m_Tank.FindAction("Move", throwIfNotFound: true);
        m_Tank_Fire = m_Tank.FindAction("Fire", throwIfNotFound: true);
        m_Tank_MouseMove = m_Tank.FindAction("MouseMove", throwIfNotFound: true);
        m_Tank_FireGameStick = m_Tank.FindAction("FireGameStick", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Tank
    private readonly InputActionMap m_Tank;
    private ITankActions m_TankActionsCallbackInterface;
    private readonly InputAction m_Tank_Move;
    private readonly InputAction m_Tank_Fire;
    private readonly InputAction m_Tank_MouseMove;
    private readonly InputAction m_Tank_FireGameStick;
    public struct TankActions
    {
        private @InputTank m_Wrapper;
        public TankActions(@InputTank wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Tank_Move;
        public InputAction @Fire => m_Wrapper.m_Tank_Fire;
        public InputAction @MouseMove => m_Wrapper.m_Tank_MouseMove;
        public InputAction @FireGameStick => m_Wrapper.m_Tank_FireGameStick;
        public InputActionMap Get() { return m_Wrapper.m_Tank; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TankActions set) { return set.Get(); }
        public void SetCallbacks(ITankActions instance)
        {
            if (m_Wrapper.m_TankActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_TankActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnMove;
                @Fire.started -= m_Wrapper.m_TankActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnFire;
                @MouseMove.started -= m_Wrapper.m_TankActionsCallbackInterface.OnMouseMove;
                @MouseMove.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnMouseMove;
                @MouseMove.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnMouseMove;
                @FireGameStick.started -= m_Wrapper.m_TankActionsCallbackInterface.OnFireGameStick;
                @FireGameStick.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnFireGameStick;
                @FireGameStick.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnFireGameStick;
            }
            m_Wrapper.m_TankActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @MouseMove.started += instance.OnMouseMove;
                @MouseMove.performed += instance.OnMouseMove;
                @MouseMove.canceled += instance.OnMouseMove;
                @FireGameStick.started += instance.OnFireGameStick;
                @FireGameStick.performed += instance.OnFireGameStick;
                @FireGameStick.canceled += instance.OnFireGameStick;
            }
        }
    }
    public TankActions @Tank => new TankActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardMouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_MobileSchemeSchemeIndex = -1;
    public InputControlScheme MobileSchemeScheme
    {
        get
        {
            if (m_MobileSchemeSchemeIndex == -1) m_MobileSchemeSchemeIndex = asset.FindControlSchemeIndex("MobileScheme");
            return asset.controlSchemes[m_MobileSchemeSchemeIndex];
        }
    }
    public interface ITankActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnMouseMove(InputAction.CallbackContext context);
        void OnFireGameStick(InputAction.CallbackContext context);
    }
}