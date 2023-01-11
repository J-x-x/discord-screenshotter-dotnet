//Functions utillizing the user32.dll 
//Documentation on user32.dll - http://www.pinvoke.net/index.aspx
using System.Runtime.InteropServices;

public static class User32
    {
    public const int SW_MAXIMIZE = 3;
    public const int SW_MINIMIZE = 6;
    // more here: http://www.pinvoke.net/default.aspx/user32.showwindow

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);


    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsIconic(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool CloseClipboard();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll")]
    public static extern bool EmptyClipboard();

    [DllImport("user32.dll")]
    public static extern IntPtr GetOpenClipboardWindow();

}