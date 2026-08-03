// JoyShockLibrary Unity bindings.
// NOT BOUND: JslSetDS5TriggersMultiPosFeedback / JslSetDS5TriggersMultiPosVibration.
// Both take a `const std::vector<unsigned char>&` parameter. std::vector has no
// stable, compiler-independent binary layout, so it cannot be safely constructed
// or passed from managed code via P/Invoke -- doing so would work by accident on
// one compiler/STL combination and corrupt memory on another. If you need these,
// add a small C-linkage wrapper in the native library taking
// `const unsigned char* values, int count` instead, and bind that.

using System;
using System.Runtime.InteropServices;

public static class JSL
{
    private const string DLL = "JoyShockLibrary";
    private const CallingConvention CC = CallingConvention.Cdecl;

    // ---------------------------------------------------------------
    // Controller type / split type
    // ---------------------------------------------------------------
    public const int TypeJoyConLeft = 1;
    public const int TypeJoyConRight = 2;
    public const int TypeProController = 3;
    public const int TypeDS4 = 4;
    public const int TypeDS = 5;

    public const int SplitTypeLeft = 1;
    public const int SplitTypeRight = 2;
    public const int SplitTypeFull = 3;

    // ---------------------------------------------------------------
    // Button masks (bit flags for JOY_SHOCK_STATE.buttons / JslGetButtons)
    // ---------------------------------------------------------------
    public const int MaskUp = 0x000001;
    public const int MaskDown = 0x000002;
    public const int MaskLeft = 0x000004;
    public const int MaskRight = 0x000008;
    public const int MaskPlus = 0x000010;
    public const int MaskOptions = 0x000010;
    public const int MaskMinus = 0x000020;
    public const int MaskShare = 0x000020;
    public const int MaskLClick = 0x000040;
    public const int MaskRClick = 0x000080;
    public const int MaskL = 0x000100;
    public const int MaskR = 0x000200;
    public const int MaskZL = 0x000400;
    public const int MaskZR = 0x000800;
    public const int MaskS = 0x001000; // South: B (Nintendo) / X (DS4)
    public const int MaskE = 0x002000; // East:  A (Nintendo) / O (DS4)
    public const int MaskW = 0x004000; // West:  Y (Nintendo) / Square (DS4)
    public const int MaskN = 0x008000; // North: X (Nintendo) / Triangle (DS4)
    public const int MaskHome = 0x010000;
    public const int MaskPS = 0x010000;
    public const int MaskCapture = 0x020000;
    public const int MaskTouchpadClick = 0x020000;
    public const int MaskMic = 0x040000;
    public const int MaskSL = 0x080000;
    public const int MaskSR = 0x100000;
    public const int MaskFNL = 0x200000;
    public const int MaskFNR = 0x400000;

    // Bit-index equivalents of the masks above (JSOFFSET_* in the header)
    public const int OffsetUp = 0;
    public const int OffsetDown = 1;
    public const int OffsetLeft = 2;
    public const int OffsetRight = 3;
    public const int OffsetPlus = 4;
    public const int OffsetOptions = 4;
    public const int OffsetMinus = 5;
    public const int OffsetShare = 5;
    public const int OffsetLClick = 6;
    public const int OffsetRClick = 7;
    public const int OffsetL = 8;
    public const int OffsetR = 9;
    public const int OffsetZL = 10;
    public const int OffsetZR = 11;
    public const int OffsetS = 12;
    public const int OffsetE = 13;
    public const int OffsetW = 14;
    public const int OffsetN = 15;
    public const int OffsetHome = 16;
    public const int OffsetPS = 16;
    public const int OffsetCapture = 17;
    public const int OffsetTouchpadClick = 17;
    public const int OffsetMic = 18;
    public const int OffsetSL = 19;
    public const int OffsetSR = 20;
    public const int OffsetFNL = 21;
    public const int OffsetFNR = 22;

    // ---------------------------------------------------------------
    // DS5 lightbar player-number values (pass to JslSetPlayerNumber)
    // ---------------------------------------------------------------
    public const int DS5Player1 = 4;
    public const int DS5Player2 = 10;
    public const int DS5Player3 = 21;
    public const int DS5Player4 = 27;
    public const int DS5Player5 = 31;

    // ---------------------------------------------------------------
    // Gyro space (JslSetGyroSpace)
    // ---------------------------------------------------------------
    public const int GyroSpaceLocal = 0;
    public const int GyroSpaceWorld = 1;
    public const int GyroSpacePlayer = 2;

    // ---------------------------------------------------------------
    // DualSense adaptive-trigger target selector
    // ---------------------------------------------------------------
    public enum EDS5AffectedTriggers : int
    {
        Both = 1,
        Left = 2,
        Right = 3
    }

    // ---------------------------------------------------------------
    // Structs -- field order matches JoyShockLibrary.h exactly
    // ---------------------------------------------------------------

    [StructLayout(LayoutKind.Sequential)]
    public struct JOY_SHOCK_STATE
    {
        public int buttons;
        public float lTrigger;
        public float rTrigger;
        public float stickLX;
        public float stickLY;
        public float stickRX;
        public float stickRY;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMU_STATE
    {
        public float accelX;
        public float accelY;
        public float accelZ;
        public float gyroX;
        public float gyroY;
        public float gyroZ;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MOTION_STATE
    {
        public float quatW;
        public float quatX;
        public float quatY;
        public float quatZ;
        public float accelX;
        public float accelY;
        public float accelZ;
        public float gravX;
        public float gravY;
        public float gravZ;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TOUCH_STATE
    {
        public int t0Id;
        public int t1Id;
        [MarshalAs(UnmanagedType.I1)] public bool t0Down;
        [MarshalAs(UnmanagedType.I1)] public bool t1Down;
        public float t0X;
        public float t0Y;
        public float t1X;
        public float t1Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JSL_AUTO_CALIBRATION
    {
        public float confidence;
        [MarshalAs(UnmanagedType.I1)] public bool autoCalibrationEnabled;
        [MarshalAs(UnmanagedType.I1)] public bool isSteady;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct JSL_SETTINGS
    {
        public int gyroSpace;
        public int colour;
        public int playerNumber;
        public int controllerType;
        public int splitType;
        [MarshalAs(UnmanagedType.I1)] public bool isCalibrating;
        [MarshalAs(UnmanagedType.I1)] public bool autoCalibrationEnabled;
        [MarshalAs(UnmanagedType.I1)] public bool isConnected;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string path;
    }

    // ---------------------------------------------------------------
    // Callback delegates
    // ---------------------------------------------------------------

    [UnmanagedFunctionPointer(CC)]
    public delegate void EventCallback(int handle, JOY_SHOCK_STATE state, JOY_SHOCK_STATE lastState,
        IMU_STATE imuState, IMU_STATE lastImuState, float deltaTime);

    [UnmanagedFunctionPointer(CC)]
    public delegate void TouchCallback(int handle, TOUCH_STATE state, TOUCH_STATE lastState, float deltaTime);

    [UnmanagedFunctionPointer(CC)]
    public delegate void ConnectCallback(int handle);

    [UnmanagedFunctionPointer(CC)]
    public delegate void DisconnectCallback(int handle, [MarshalAs(UnmanagedType.I1)] bool timedOut);

    // ---------------------------------------------------------------
    // Connection management
    // ---------------------------------------------------------------

    [DllImport(DLL, CallingConvention = CC)]
    public static extern int JslConnectDevices();
    [DllImport(DLL, CallingConvention = CC)]
    public static extern int JslGetConnectedDeviceHandles(int[] deviceHandleArray, int size);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslDisconnectAndDisposeAll();
    [DllImport(DLL, CallingConvention = CC)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool JslStillConnected(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslDisconnect(int deviceId);

    // ---------------------------------------------------------------
    // Bulk state getters -- preferred over the individual getters below
    // ---------------------------------------------------------------

    [DllImport(DLL, CallingConvention = CC)]
    public static extern JOY_SHOCK_STATE JslGetSimpleState(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern IMU_STATE JslGetIMUState(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern MOTION_STATE JslGetMotionState(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern TOUCH_STATE JslGetTouchState(int deviceId, [MarshalAs(UnmanagedType.I1)] bool previous);
    [DllImport(DLL, CallingConvention = CC)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool JslGetTouchpadDimension(int deviceId, ref int sizeX, ref int sizeY);

    // ---------------------------------------------------------------
    // Individual state getters
    // ---------------------------------------------------------------

    [DllImport(DLL, CallingConvention = CC)]
    public static extern int JslGetButtons(int deviceId);

    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetLeftX(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetLeftY(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetRightX(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetRightY(int deviceId);

    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetLeftTrigger(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetRightTrigger(int deviceId);

    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetGyroX(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetGyroY(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetGyroZ(int deviceId);

    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslGetAndFlushAccumulatedGyro(int deviceId, ref float gyroX, ref float gyroY, ref float gyroZ);

    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetGyroSpace(int deviceId, int gyroSpace);

    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetAccelX(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetAccelY(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetAccelZ(int deviceId);

    [DllImport(DLL, CallingConvention = CC)]
    public static extern int JslGetTouchId(int deviceId, [MarshalAs(UnmanagedType.I1)] bool secondTouch);
    [DllImport(DLL, CallingConvention = CC)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool JslGetTouchDown(int deviceId, [MarshalAs(UnmanagedType.I1)] bool secondTouch);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetTouchX(int deviceId, [MarshalAs(UnmanagedType.I1)] bool secondTouch);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetTouchY(int deviceId, [MarshalAs(UnmanagedType.I1)] bool secondTouch);

    // ---------------------------------------------------------------
    // Device characteristics
    // ---------------------------------------------------------------

    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetStickStep(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetTriggerStep(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetPollRate(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern float JslGetTimeSinceLastUpdate(int deviceId);

    // ---------------------------------------------------------------
    // Calibration
    // ---------------------------------------------------------------

    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslResetContinuousCalibration(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslStartContinuousCalibration(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslPauseContinuousCalibration(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetAutomaticCalibration(int deviceId, [MarshalAs(UnmanagedType.I1)] bool enabled);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslGetCalibrationOffset(int deviceId, ref float xOffset, ref float yOffset, ref float zOffset);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetCalibrationOffset(int deviceId, float xOffset, float yOffset, float zOffset);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern JSL_AUTO_CALIBRATION JslGetAutoCalibrationStatus(int deviceId);

    // ---------------------------------------------------------------
    // Callbacks
    // ---------------------------------------------------------------

    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetCallback(EventCallback callback);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetTouchCallback(TouchCallback callback);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetConnectCallback(ConnectCallback callback);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetDisconnectCallback(DisconnectCallback callback);

    // ---------------------------------------------------------------
    // Device info / colour / rumble
    // ---------------------------------------------------------------

    [DllImport(DLL, CallingConvention = CC)]
    public static extern JSL_SETTINGS JslGetControllerInfoAndSettings(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern int JslGetControllerType(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern int JslGetControllerSplitType(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern int JslGetControllerBodyColour(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern int JslGetControllerLeftGripColour(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern int JslGetControllerRightGripColour(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern int JslGetControllerButtonColour(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetLightColour(int deviceId, int colour);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetRumble(int deviceId, int smallRumble, int bigRumble);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetPlayerNumber(int deviceId, int number);

    // ---------------------------------------------------------------
    // DualSense (DS5) adaptive trigger effects
    // position/strength/frequency parameters use raw byte ranges -- see the
    // XML doc comments in JoyShockLibrary.h for each function's valid ranges
    // ---------------------------------------------------------------

    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetDS5TriggersOff(int deviceId, EDS5AffectedTriggers affectedTriggers);

    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetDS5TriggersFeedback(int deviceId, EDS5AffectedTriggers affectedTriggers,
        byte position, byte strength);

    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetDS5TriggersWeapon(int deviceId, EDS5AffectedTriggers affectedTriggers,
        byte startPosition, byte endPosition, byte strength);

    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetDS5TriggersVibration(int deviceId, EDS5AffectedTriggers affectedTriggers,
        byte position, byte amplitude, byte frequency);

    // JslSetDS5TriggersMultiPosFeedback intentionally not bound -- see file header comment.

    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetDS5TriggersSlopeFeedback(int deviceId, EDS5AffectedTriggers affectedTriggers,
        byte startPosition, byte endPosition, byte startStrength, byte endStrength);

    // JslSetDS5TriggersMultiPosVibration intentionally not bound -- see file header comment.

    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetDS5TriggersBow(int deviceId, EDS5AffectedTriggers affectedTriggers,
        byte startPosition, byte endPosition, byte strength, byte snapForce);

    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetDS5TriggersGalloping(int deviceId, EDS5AffectedTriggers affectedTriggers,
        byte startPosition, byte endPosition, byte firstFoot, byte secondFoot, byte frequency);

    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetDS5TriggersMachine(int deviceId, EDS5AffectedTriggers affectedTriggers,
        byte startPosition, byte endPosition, byte amplitudeA, byte amplitudeB, byte frequency, byte period);

    // ---------------------------------------------------------------
    // Joy-Con / Switch controller HD Rumble
    // ---------------------------------------------------------------

    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslEnableHDRumble(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslDisableHDRumble(int deviceId);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetHDRumble(int deviceId, float lowFreq, float lowAmpli, float highFreq, float highAmpli);
    [DllImport(DLL, CallingConvention = CC)]
    public static extern void JslSetHDRumbleLR(int deviceId,
        float lowFreqL, float lowAmpliL, float highFreqL, float highAmpliL,
        float lowFreqR, float lowAmpliR, float highFreqR, float highAmpliR);
}
