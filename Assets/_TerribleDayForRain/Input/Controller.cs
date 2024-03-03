//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/_TerribleDayForRain/Input/Controller.inputactions
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

public partial class @Controller: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controller()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controller"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""5acbbb67-d39a-46c8-91a3-0ea3f1b1f48e"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""d4747261-8630-403e-afc1-78dd954662e3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""53627821-d7ea-4b03-a34b-8753b810ae18"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Basic Skill"",
                    ""type"": ""Button"",
                    ""id"": ""9a6d1cbd-a590-4d8c-aa2f-4fa5b6bc4313"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AoeSkill"",
                    ""type"": ""Button"",
                    ""id"": ""dae3017e-ffdd-43b7-9ab9-e140b0699359"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SummonSkill"",
                    ""type"": ""Button"",
                    ""id"": ""c8e7fbd9-9d42-4786-8e19-614369b5e027"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MovementSkill"",
                    ""type"": ""Button"",
                    ""id"": ""9fe472a2-6e61-4cd7-a972-7862ad53e57a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""89fe0048-ac64-4fce-96ea-ba75aa431800"",
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
                    ""id"": ""4353baf5-e21c-4c38-a8cb-76956100408a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Basic Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ef424df-c8c3-4c29-ad99-8b9e42e391a8"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Basic Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3fa09d4e-f7a0-4fdb-9aef-d6708ba9e53a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AoeSkill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""350d69f8-5e55-461a-9ec8-0fcfd2f92e0d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SummonSkill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""cea03efa-01cc-4196-8b81-5d5e1e987011"",
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
                    ""id"": ""efdeee4c-584d-424f-9eb3-aad9a87aeed6"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""be2eca82-d879-40c9-b208-12269c9d253a"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c00b562d-c206-48b6-be5d-28ccfed2904c"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bd80517e-d318-446f-8bd1-ed1c98d06185"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""0797df14-5972-48ee-80c8-7a118eb1b51e"",
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
                    ""id"": ""2b4be3d9-9e0e-40f5-8f85-f7067f94d9fb"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""16a35cd5-ea80-4f83-80c5-03e7c5ddcfce"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""aad8a437-bd88-45cd-a0d7-a8ab9756e965"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""50e91e96-7db1-4ebe-a321-74d016a66510"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""03b0b8f0-dbe9-4867-bc7d-dcc7f1e48553"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""MultiTap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementSkill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3407646a-91ea-4134-9c5d-f3bc93ee200b"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementSkill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_BasicSkill = m_Gameplay.FindAction("Basic Skill", throwIfNotFound: true);
        m_Gameplay_AoeSkill = m_Gameplay.FindAction("AoeSkill", throwIfNotFound: true);
        m_Gameplay_SummonSkill = m_Gameplay.FindAction("SummonSkill", throwIfNotFound: true);
        m_Gameplay_MovementSkill = m_Gameplay.FindAction("MovementSkill", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_BasicSkill;
    private readonly InputAction m_Gameplay_AoeSkill;
    private readonly InputAction m_Gameplay_SummonSkill;
    private readonly InputAction m_Gameplay_MovementSkill;
    public struct GameplayActions
    {
        private @Controller m_Wrapper;
        public GameplayActions(@Controller wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @BasicSkill => m_Wrapper.m_Gameplay_BasicSkill;
        public InputAction @AoeSkill => m_Wrapper.m_Gameplay_AoeSkill;
        public InputAction @SummonSkill => m_Wrapper.m_Gameplay_SummonSkill;
        public InputAction @MovementSkill => m_Wrapper.m_Gameplay_MovementSkill;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void AddCallbacks(IGameplayActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @BasicSkill.started += instance.OnBasicSkill;
            @BasicSkill.performed += instance.OnBasicSkill;
            @BasicSkill.canceled += instance.OnBasicSkill;
            @AoeSkill.started += instance.OnAoeSkill;
            @AoeSkill.performed += instance.OnAoeSkill;
            @AoeSkill.canceled += instance.OnAoeSkill;
            @SummonSkill.started += instance.OnSummonSkill;
            @SummonSkill.performed += instance.OnSummonSkill;
            @SummonSkill.canceled += instance.OnSummonSkill;
            @MovementSkill.started += instance.OnMovementSkill;
            @MovementSkill.performed += instance.OnMovementSkill;
            @MovementSkill.canceled += instance.OnMovementSkill;
        }

        private void UnregisterCallbacks(IGameplayActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @BasicSkill.started -= instance.OnBasicSkill;
            @BasicSkill.performed -= instance.OnBasicSkill;
            @BasicSkill.canceled -= instance.OnBasicSkill;
            @AoeSkill.started -= instance.OnAoeSkill;
            @AoeSkill.performed -= instance.OnAoeSkill;
            @AoeSkill.canceled -= instance.OnAoeSkill;
            @SummonSkill.started -= instance.OnSummonSkill;
            @SummonSkill.performed -= instance.OnSummonSkill;
            @SummonSkill.canceled -= instance.OnSummonSkill;
            @MovementSkill.started -= instance.OnMovementSkill;
            @MovementSkill.performed -= instance.OnMovementSkill;
            @MovementSkill.canceled -= instance.OnMovementSkill;
        }

        public void RemoveCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnBasicSkill(InputAction.CallbackContext context);
        void OnAoeSkill(InputAction.CallbackContext context);
        void OnSummonSkill(InputAction.CallbackContext context);
        void OnMovementSkill(InputAction.CallbackContext context);
    }
}
