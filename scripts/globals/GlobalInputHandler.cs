using Godot;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public partial class GlobalInputHandler : Node
{
    [Signal]
    public delegate void KeyPressedEventHandler(int keyCode);
    public enum KeyCode : int
    {
        Backspace = 0x08,
        Tab = 0x09,
        Enter = 0x0D,
        //NumPadEnter = 0x0D, - why is this so stupid?
        Pause = 0x13,
        CapsLock = 0x14,
        Escape = 0x1B,
        Space = 0x20,
        PageUp = 0x21,
        PageDown = 0x22,
        End = 0x23,
        Home = 0x24,
        ArrowLeft = 0x25,
        ArrowUp = 0x26,
        ArrowRight = 0x27,
        ArrowDown = 0x28,
        PrintScreen = 0x2C,
        Insert = 0x2D,
        Delete = 0x2E,
        Num0 = 0x30,
        Num1 = 0x31,
        Num2 = 0x32,
        Num3 = 0x33,
        Num4 = 0x34,
        Num5 = 0x35,
        Num6 = 0x36,
        Num7 = 0x37,
        Num8 = 0x38,
        Num9 = 0x39,
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,
        LeftWin = 0x5B,
        RightWin = 0x5C,
        Apps = 0x5D,
        Menu = 0x5D,
        NumPad0 = 0x60,
        NumPad1 = 0x61,
        NumPad2 = 0x62,
        NumPad3 = 0x63,
        NumPad4 = 0x64,
        NumPad5 = 0x65,
        NumPad6 = 0x66,
        NumPad7 = 0x67,
        NumPad8 = 0x68,
        NumPad9 = 0x69,
        NumPadMultiply = 0x6A,
        NumPadAdd = 0x6B,
        NumPadDecimal = 0x6E,
        NumPadSubtract = 0x6D,
        NumPadDivide = 0x6F,
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        NumLock = 0x90,
        ScrollLock = 0x91,
        LeftBracket = 0xDB,
        RightBracket = 0xDD,
        Semicolon = 0xBA,
        Equals = 0xBB,
        Comma = 0xBC,
        Minus = 0xBD,
        Period = 0xBE,
        Slash = 0xBF,
        Backquote = 0xC0,
        Tilde = 0xC0,
        Backslash = 0xDC,
        Quote = 0xDE,
        LShift = 0xA0,
        RShift = 0xA1,
        LControl = 0xA2,
        RControl = 0xA3,
        LAlt = 0xA4,
        RAlt = 0xA5
    }
    public override void _Ready()
    {
        proc = HookCallback;
        hookId = SetHook(proc);
        AddUserSignal(
            nameof(KeyPressedEventHandler), 
            new Godot.Collections.Array()
            {
                new Godot.Collections.Dictionary(){
                    { "name", "keyCode" },
                    { "type", (int)Variant.Type.Int }
                }
            }
        );
    }

    public override void _ExitTree()
    {
        UnhookWindowsHookEx(hookId);
    }
    // communication with the rest of the game
    // low level stuff
    private IntPtr hookId = IntPtr.Zero;
    private const int WH_KEYBOARD_LL = 13;
    private LowLevelKeyboardProc proc;

    private IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
        {
            int vkCode = Marshal.ReadInt32(lParam);
            GD.Print($"Unfocused key pressed: {(KeyCode)vkCode}");
            EmitSignal(nameof(KeyPressedEventHandler), vkCode);

        }
        return CallNextHookEx(hookId, nCode, wParam, lParam);
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;
}
