using UnityEngine;
using System.Collections;

public delegate void ButtonPressHandler(bool isPressed);

public class Button : MonoBehaviour {

    public Color color;
    public Material unpressedMat, pressedMat;

    public event ButtonPressHandler buttonListeners;

    private float lightMax = 1, lightMin = .5f;
    private Color dim;
    private bool isPressed = false;
    private float pulseRate = 2;
    private Interpolate.Function
        easeDown = Interpolate.Ease(Interpolate.EaseType.EaseOutCubic),
        easeUp = Interpolate.Ease(Interpolate.EaseType.Linear);
    private Light buttonLight;
    private MeshRenderer buttonRenderer;

    private float buttonPressStart = -1;
    private float buttonPressDuration = .25f;
    private float buttonResetStart = -1f;
    private Vector3 buttonPosOn, buttonPosOff, buttonPosBottom;
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
        dim = color;
        dim /= 1.5f;
        dim.a = 1;
        buttonLight = GetComponentInChildren<Light>();
        buttonLight.color = color;
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
        PulseButton();
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
    private void PulseButton() {
        if(IsPressed) {
            return;
        }
        float percent = Time.time / pulseRate % 1;
        if(percent < .5f) {
            buttonLight.intensity = Mathf.Lerp(lightMax, lightMin, percent / .5f);
            buttonRenderer.sharedMaterial.color = Color.Lerp(color, dim, percent / .5f);
        } else {
            buttonLight.intensity = Mathf.Lerp(lightMin, lightMax, (percent-.5f) / .5f);
            buttonRenderer.sharedMaterial.color = Color.Lerp(dim, color, (percent-.5f) / .5f);
        }
    }
    public void ResetButton() {
        IsPressed = false;
    }
    public void ToggleButton() {
        IsPressed = !IsPressed;
    }

    private void UpdateButton(bool silent=false) {
        if(isPressed) {
            buttonLight.enabled = false;
            buttonRenderer.material = pressedMat;
            if(!silent) {
                buttonPressStart = Time.time;
            }
        } else {
            buttonLight.enabled = true;
            buttonRenderer.material = unpressedMat;
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
