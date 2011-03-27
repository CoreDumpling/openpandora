using System;
using System.Data;
using System.Security;
using System.Threading;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace OpenPandora
{
	/// <summary>
	/// Fake Winamp IPC stub.
	/// </summary>
	public class Winamp
	{
        // Winamp singleton -- only one instance needed application-wide
        private static Winamp instance = null;
        public static Winamp GetInstance()
        {
            if (instance == null)
            {
                instance = new Winamp();
            }
            return instance;
        }

        private const string WINAMP_CLASS = "Winamp v1.x";

        private IntPtr hWnd;

        private delegate IntPtr WindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        private WindowProc winampWindowProc;

        #region Unmanaged Interface

        [StructLayout(LayoutKind.Sequential)]
        struct WNDCLASS
        {
            public uint style;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public WindowProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszClassName;
        }

        [SuppressUnmanagedCodeSecurityAttribute()]
        [DllImport("User32.dll", SetLastError = true)]
        static extern IntPtr CreateWindowExW(
           UInt32 dwExStyle,
           [MarshalAs(UnmanagedType.LPWStr)] string lpClassName,
           [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName,
           UInt32 dwStyle,
           Int32 x,
           Int32 y,
           Int32 nWidth,
           Int32 nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam
        );

        [SuppressUnmanagedCodeSecurityAttribute()]
        [DllImport("User32.dll")]
        static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [SuppressUnmanagedCodeSecurityAttribute()]
        [DllImport("User32.dll", SetLastError = true)]
        static extern UInt16 RegisterClassW(ref WNDCLASS lpWndClass);

        #endregion

		// Constructor
		public Winamp()
		{
            this.winampWindowProc = new WindowProc(WinampWindowProc);

            WNDCLASS wndClass = new WNDCLASS();
            wndClass.lpfnWndProc = this.winampWindowProc;
            wndClass.lpszClassName = WINAMP_CLASS;
            wndClass.hInstance = Marshal.GetHINSTANCE(this.GetType().Module);
            UInt16 atom = RegisterClassW(ref wndClass);

            if (atom == 0)
            {
                Debug.WriteLine("Could not register window class \"Winamp v1.x\" -- bailing");
            }
            else
            {
                hWnd = CreateWindowExW(0, WINAMP_CLASS, "Pandora", 0, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                if (hWnd == IntPtr.Zero)
                {
                    Debug.WriteLine("Could not create window: error " + Marshal.GetLastWin32Error());
                }
                else
                {
                    Debug.WriteLine("Created window: " + hWnd);
                }
            }
		}

        // Window message handler
        private static IntPtr WinampWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            Debug.WriteLine("WinampWindowProc called: message type " + msg);
            return DefWindowProc(hWnd, msg, wParam, lParam);
        }
	}
}
