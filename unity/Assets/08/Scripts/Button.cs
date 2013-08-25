using UnityEngine;
using System.Collections;

public delegate void ButtonPressHandler(bool isPressed);

public class Button : MonoBehaviour {

    public Color color;
    public Material unpressedMat, pressedMat;

    public event ButtonPressHandler buttonListeners;

    private bool isPressed = false;
    private Interpolate.Function
        easeDown = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic),
        easeUp = Interpolate.Ease(Interpolate.EaseType.Linear);
    private MeshRenderer buttonRenderer;

    private float buttonPressStart = -1;
    private float buttonPressDuration = .25f;
    private float buttonResetStart = -1f;
    private Vector3 buttonPosOn, buttonPosOff, buttonPosBottom;
    private Pulse pulse;
    private Transform button;

    public bool IsPressed {
        get { return isPressed; }
        set {
            if(value == isPressed)
                return;
            isPressed = value;
            UpdateButton();
        }
    }

    public void Start() {
        pulse = GetComponent<Pulse>();
        pulse.SetColor(color);
        buttonRenderer = GetComponentInChildren<MeshRenderer>();
        // duplicate the materials so our mods don't affect other buttons
        unpressedMat = new Material(unpressedMat);
        unpressedMat.name = name + "-unpressed";
        pressedMat = new Material(pressedMat);
        pressedMat.name = name + "-pressed";
        pressedMat.color = color;
        
        button = transform.Find("buttonMesh");
        buttonPosOn = button.localPosition;
        buttonPosOff = new Vector3(0, -.3f, 0);
        buttonPosBottom = new Vector3(0, -.4f, 0);
        UpdateButton(true);
    }
    public void Update() {
        HandleDebug();
        HandlePressingButton();
        HandleResettingButton();
    }

    private void HandleDebug() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ResetButton();
        }
    }
    private void HandlePressingButton() {
        if(buttonPressStart == -1) {
            return;
        }
        float percent = (Time.time - buttonPressStart) / buttonPressDuration;
        if(percent >= 1) {
            buttonPressStart = -1;
            button.localPosition = buttonPosOff;
            return;
        }
        if(percent < .5f) {
            button.localPosition = Interpolate.Ease(easeDown, buttonPosOn,
                buttonPosBottom - buttonPosOn, percent, .5f);
        } else {
            // invoke the press if we haven't yet
            if(!IsPressed) {
                IsPressed = true;
            }
            button.localPosition = Interpolate.Ease(easeUp, buttonPosBottom,
                buttonPosOff - buttonPosBottom, percent-.5f, .5f);
        }
    }
    private void HandleResettingButton() {
        if(buttonResetStart == -1) {
            return;
        }
        float percent = (Time.time - buttonResetStart) / buttonPressDuration;
        if(percent >= 1) {
            buttonResetStart = -1f;
            button.localPosition = buttonPosOn;
            IsPressed = false;
        } else {
            button.localPosition = Interpolate.Ease(easeUp, buttonPosOff,
                buttonPosOn - buttonPosOff, percent, 1);
        }
    }
    public void PressButton() {
        IsPressed = true;
    }
    public void ResetButton() {
        IsPressed = false;
    }
    public void ToggleButton() {
        IsPressed = !IsPressed;
    }

    private void UpdateButton(bool silent=false) {
        if(isPressed) {
            pulse.enabled = false;
            buttonRenderer.material = pressedMat;
            if(!silent) {
                buttonPressStart = Time.time;
            }
        } else {
            buttonRenderer.material = unpressedMat;
            pulse.enabled = true;
            if(!silent) {
                buttonResetStart = Time.time;
            }
        }
        if(!silent && buttonListeners != null) {
            buttonListeners(isPressed);
        }
    }

    public void OnTriggerEnter(Collider other) {
        if(!IsPressed) {
            PressButton();
        }
    }
}
