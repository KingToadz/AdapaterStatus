using System;
using System.Runtime.InteropServices;
using System.Media;

namespace CheckForAdapter
{
    class Program
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_POWER_STATUS
        {
            public byte ACLineStatus;
            public byte BatteryFlag;
            public byte BatteryLifePercent;
            public byte Reserved1;
            public int BatteryLifeTime;
            public int BatteryFullLifeTime;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool GetSystemPowerStatus([In, Out] ref SYSTEM_POWER_STATUS systemPowerStatus);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            Console.Title = "Easy to find title!"; // Set the title to find it.

            IntPtr hWnd = FindWindow(null, "Easy to find title!"); // Get console handle

            if (hWnd != IntPtr.Zero)
                ShowWindow(hWnd, 0); // hide console

            SYSTEM_POWER_STATUS status = new SYSTEM_POWER_STATUS(); // The system power charger struct

            int current = 0;
            int last = 0;

            while (true)
            {
                System.Threading.Thread.Sleep(100);
                GetSystemPowerStatus(ref status); // use winapi.

                Console.WriteLine(status.ACLineStatus); // Write for debug

                current = status.ACLineStatus; // Set current to the status

                if (current != last) // If the state has changed
                {
                    if (status.ACLineStatus == 0) // running on battery
                        (new SoundPlayer("sound\\doh.wav")).Play(); // play doh
                    else if (status.ACLineStatus == 1) // running on AC
                        (new SoundPlayer("sound\\woohoo.wav")).Play(); // play woohoo
                }

                last = current; // set last to current
            }
        }
    }
}
