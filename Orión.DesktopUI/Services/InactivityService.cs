using Orión.DesktopUI.Interfaces;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace Orión.DesktopUI.Services;

public class InactivityService : IInactivityService
{
    private readonly DispatcherTimer _timer;
    private const int InactivityThresholdMinutes = 30; // Configurable en el futuro

    public event Action? OnInactivityTimeout;

    [StructLayout(LayoutKind.Sequential)]
    struct LASTINPUTINFO
    {
        public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));
        [MarshalAs(UnmanagedType.U4)]
        public uint cbSize;
        [MarshalAs(UnmanagedType.U4)]
        public uint dwTime;
    }

    [DllImport("user32.dll")]
    static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

    public InactivityService()
    {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(10); // Revisar cada 10 segundos
        _timer.Tick += Timer_Tick;
    }

    public void StartMonitoring()
    {
        _timer.Start();
    }

    public void StopMonitoring()
    {
        _timer.Stop();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        uint idleTime = GetIdleTime();
        if (idleTime > (InactivityThresholdMinutes * 60 * 1000))
        {
            OnInactivityTimeout?.Invoke();
        }
    }

    private uint GetIdleTime()
    {
        LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
        lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
        lastInputInfo.dwTime = 0;

        uint envTicks = (uint)Environment.TickCount;

        if (GetLastInputInfo(ref lastInputInfo))
        {
            uint lastInputTick = lastInputInfo.dwTime;
            return envTicks - lastInputTick;
        }
        return 0;
    }
}
