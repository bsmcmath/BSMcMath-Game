using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class input : MonoBehaviour
{
    public GameObject p1go, p2go;
    public PlayerInput p1, p2;
    public inputRouter inputP1, inputP2;
    public bool p1_joined, p2_joined;
    public bool p1_keyboard, p2_keyboard;
    void OnPlayerJoined(PlayerInput pi)
    {
        if (p1_joined) addPlayer2(pi); 
        else addPlayer1(pi);
    }
    void addPlayer1(PlayerInput pi)
    {
        p1 = pi; p1go = pi.gameObject; p1_joined = true;
        pi.SwitchCurrentActionMap(pi.currentControlScheme);
        switch (pi.currentControlScheme)
        {
            case "Keyboard":
                foreach (InputAction ia in pi.actions) { ia.performed += ctx => KeyboardInputP1(ctx); ia.canceled += ctx => KeyboardInputP1(ctx); }
                p1_keyboard = true;
                break;
            case "Gamepad":
                foreach (InputAction ia in pi.actions) { ia.performed += ctx => GamepadInputP1(ctx); ia.canceled += ctx => GamepadInputP1(ctx); }
                break;
        }
    }
    void addPlayer2(PlayerInput pi)
    {
        p2 = pi; p2go = pi.gameObject; p2_joined = true;
        pi.SwitchCurrentActionMap(pi.currentControlScheme);
        switch (pi.currentControlScheme)
        {
            case "Keyboard":
                foreach (InputAction ia in pi.actions) { ia.performed += ctx => KeyboardInputP2(ctx); ia.canceled += ctx => KeyboardInputP2(ctx); }
                p2_keyboard = true;
                break;
            case "Gamepad":
                foreach (InputAction ia in pi.actions) { ia.performed += ctx => GamepadInputP2(ctx); ia.canceled += ctx => GamepadInputP2(ctx); }
                break;
        }
    }
    void OnPlayerLeft(PlayerInput pi)
    {
        if (pi == p1)
        {
            p1_joined = false; 
            p1_keyboard = false;
            //Debug.Log("player1Leave");
        }
        else if (pi == p2)
        {
            p2_joined = false; 
            p2_keyboard = false;
            //Debug.Log("player2Leave");
        }
    }
    private void Update()
    {
        if (p1_keyboard) keyboardP1();
        if (p2_keyboard) keyboardP2();
    }
    bool w, a, s, d;
    public void keyboardP1()
    {
        inputP1.turn = Mouse.current.delta.ReadValue() * 0.1f;
        inputP1.push = Vector3.zero;
        if (w) inputP1.push.y += 1;
        if (a) inputP1.push.x -= 1;
        if (s) inputP1.push.y -= 1;
        if (d) inputP1.push.x += 1;
        if (inputP1.push.magnitude > 1) inputP1.push = inputP1.push.normalized;
    }
    public void keyboardP2()
    {
        inputP2.turn = Mouse.current.delta.ReadValue() * 0.1f;
        inputP2.push = Vector3.zero;
        if (w) inputP2.push.y += 1;
        if (a) inputP2.push.x -= 1;
        if (s) inputP2.push.y -= 1;
        if (d) inputP2.push.x += 1;
        if (inputP2.push.magnitude > 1) inputP2.push = inputP2.push.normalized;
    }
    public void KeyboardInputP1(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "W":
                w = context.performed;
                break;
            case "A":
                a = context.performed;
                break;
            case "S":
                s = context.performed;
                break;
            case "D":
                d = context.performed;
                break;
            case "LeftClick":
                inputP1.rightTrigger = context.performed;
                break;
            case "RightClick":
                inputP1.rightBump = context.performed;
                break;
            case "Space":
                inputP1.jump = context.performed;
                break;
            case "LeftShift":
                inputP1.leftTrigger = context.performed;
                break;
            case "LeftControl":
                inputP1.leftBump = context.performed;
                break;
            case "Q":
                inputP1.dodge = context.performed;
                break;
            case "E":
                inputP1.interact = context.performed;
                break;
            case "R":
                inputP1.down = context.performed;
                break;
            case "F":
                inputP1.crouch = context.performed;
                break;
            case "1":
                inputP1.left = context.performed;
                break;
            case "2":
                inputP1.up = context.performed;
                break;
            case "3":
                inputP1.right = context.performed;
                break;
            case "Escape":
                inputP1.start = context.performed;
                break;
            case "Z":
                inputP1.select = context.performed;
                break;
        }
    }
    public void KeyboardInputP2(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "W":
                w = context.performed;
                break;
            case "A":
                a = context.performed;
                break;
            case "S":
                s = context.performed;
                break;
            case "D":
                d = context.performed;
                break;
            case "LeftClick":
                inputP2.rightTrigger = context.performed;
                break;
            case "RightClick":
                inputP2.rightBump = context.performed;
                break;
            case "Space":
                inputP2.jump = context.performed;
                break;
            case "LeftShift":
                inputP2.leftTrigger = context.performed;
                break;
            case "LeftControl":
                inputP2.leftBump = context.performed;
                break;
            case "Q":
                inputP2.dodge = context.performed;
                break;
            case "E":
                inputP2.interact = context.performed;
                break;
            case "R":
                inputP2.down = context.performed;
                break;
            case "F":
                inputP2.crouch = context.performed;
                break;
            case "1":
                inputP2.left = context.performed;
                break;
            case "2":
                inputP2.up = context.performed;
                break;
            case "3":
                inputP2.right = context.performed;
                break;
            case "Escape":
                inputP2.start = context.performed;
                break;
            case "Z":
                inputP2.crouch = context.performed;
                break;
        }
    }
    public void GamepadInputP1(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "LeftStick":
                inputP1.push = context.ReadValue<Vector2>();
                break;
            case "RightStick":
                inputP1.turn = context.ReadValue<Vector2>();
                break;
            case "LeftBumper":
                inputP1.leftBump = context.performed;
                break;
            case "RightBumper":
                inputP1.rightBump = context.performed;
                break;
            case "LeftTrigger":
                inputP1.leftTrigger = context.performed;
                break;
            case "RightTrigger":
                inputP1.rightTrigger = context.performed;
                break;
            case "Start":
                inputP1.start = context.performed;
                break;
            case "Select":
                inputP1.select = context.performed;
                break;
            case "North":
                inputP1.jump = context.performed;
                break;
            case "South":
                inputP1.interact = context.performed;
                break;
            case "East":
                inputP1.crouch = context.performed;
                break;
            case "West":
                inputP1.dodge = context.performed;
                break;
            case "Up":
                inputP1.up = context.performed;
                break;
            case "Down":
                inputP1.down = context.performed;
                break;
            case "Left":
                inputP1.left = context.performed;
                break;
            case "Right":
                inputP1.right = context.performed;
                break;
            case "LeftStickPress":
                inputP1.hurry = context.performed;
                break;
            case "RightStickPress":
                inputP1.toggle = context.performed;
                break;
        }
    }
    public void GamepadInputP2(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "LeftStick":
                inputP2.push = context.ReadValue<Vector2>();
                break;
            case "RightStick":
                inputP2.turn = context.ReadValue<Vector2>();
                break;
            case "LeftBumper":
                inputP2.leftBump = context.performed;
                break;
            case "RightBumper":
                inputP2.rightBump = context.performed;
                break;
            case "LeftTrigger":
                inputP2.leftTrigger = context.performed;
                break;
            case "RightTrigger":
                inputP2.rightTrigger = context.performed;
                break;
            case "Start":
                inputP2.start = context.performed;
                break;
            case "Select":
                inputP2.select = context.performed;
                break;
            case "North":
                inputP2.jump = context.performed;
                break;
            case "South":
                inputP2.interact = context.performed;
                break;
            case "East":
                inputP2.crouch = context.performed;
                break;
            case "West":
                inputP2.dodge = context.performed;
                break;
            case "Up":
                inputP2.up = context.performed;
                break;
            case "Down":
                inputP2.down = context.performed;
                break;
            case "Left":
                inputP2.left = context.performed;
                break;
            case "Right":
                inputP2.right = context.performed;
                break;
            case "LeftStickPress":
                inputP2.hurry = context.performed;
                break;
            case "RightStickPress":
                inputP2.toggle = context.performed;
                break;
        }
    }
}
