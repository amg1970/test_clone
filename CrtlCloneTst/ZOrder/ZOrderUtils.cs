using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MVisionPlayer.ZOrder
{
    //mm: ZOrderUtils is a class that contains a set of native methods used by ZOrderMng. 
    //Note: To maintain the autonomy of ZOrder-module some native methods declarations are duplicated with that one in MVision.Helpers.WinApi
    public static partial class ZOrderUtils
    {
        /*
        #region constants

        public const short SWP_NOMOVE = 0x2;
        public const short SWP_NOSIZE = 1;
        public const int SWP_SHOWWINDOW = 0x0040;

        public const int SWP_SWP_ASYNCWINDOWPOS = 0x4000;

        public const int SWP_NOCOPYBITS = 0x0100;

        public const int SWP_NOZORDER = 0x0004;

        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        public const uint TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        #endregion

        #region externals declarations

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("wininet.dll")]
        public static extern bool InternetGetConnectedState(out int Description, int ReservedValue);

        [DllImport("user32.dll")]
        public static extern int ShowCursor(bool bShow);

        [DllImport("User32.dll")]
        public static extern IntPtr GetParent(IntPtr hwnd);

        // The GetWindowRect function takes a handle to the window as the first parameter. The second parameter
        // must include a reference to a Rectangle object. This Rectangle object will then have it's values set
        // to the window rectangle properties.
        [DllImport("user32.dll")]
        public static extern long GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);
      */

        #region constants

        public const short SWP_NOMOVE = 0x2;
        public const short SWP_NOSIZE = 1;
        public const int SWP_SHOWWINDOW = 0x0040;

        public const int SWP_SWP_ASYNCWINDOWPOS = 0x4000;

        public const int SWP_NOCOPYBITS = 0x0100;

        public const int SWP_NOZORDER = 0x0004;

        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        public const uint TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        #endregion

        #region externals declarations

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("wininet.dll")]
        public static extern bool InternetGetConnectedState(out int Description, int ReservedValue);

        [DllImport("user32.dll")]
        public static extern int ShowCursor(bool bShow);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(int hWnd, int nCmdShow);

        //mm
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        //[DllImport("user32.dll")]
        //static extern bool ShowWindowAsync(int hWnd, int nCmdShow);
        [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
        static extern int SetLayeredWindowAttributes(int hwnd, int crKey, byte bAlpha, int dwFlags);

        //[DllImport("user32")]
        //private static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);

        //[DllImport("user32")]
        //private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        //public const int SWP_NOZORDER = 0x0004;
        public const uint SWP_NOACTIVATE = 0x0010;
        public const int GWL_STYLE = -16;
        public const int WS_CAPTION = 0x00C00000;
        public const int WS_THICKFRAME = 0x00040000;
        public const int WS_CHILD = 0x40000000;
        //public const int WS_POPUP = 0x80000000;

        public const int WS_CLIPCHILDREN = 0x2000000;
        public const int WS_CLIPSIBLINGS = 0x4000000;


        //public const long WS_EX_TRANSPARENT = 0x20000000;

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_LAYERED = 0x80000;
        public const int LWA_ALPHA = 0x2;

        // The GetWindowRect function takes a handle to the window as the first parameter. The second parameter
        // must include a reference to a Rectangle object. This Rectangle object will then have it's values set
        // to the window rectangle properties.
        [DllImport("user32.dll")]
        public static extern long GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);


        //-------------------
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        // This static method is required because Win32 does not support
        // GetWindowLongPtr directly
        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }

        // SetWindowLongPtr 
        //public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
          if (IntPtr.Size == 8)
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
          else
           return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint="SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);
        //private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint="SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        //private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);


        //-------------------

        // This helper static method is required because the 32-bit version of user32.dll does not contain this API
        // (on any versions of Windows), so linking the method will fail at run-time. The bridge dispatches the request
        // to the correct function (GetWindowLong in 32-bit mode and GetWindowLongPtr in 64-bit mode)
        /*
        public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);
        */
        #endregion


    }
}
