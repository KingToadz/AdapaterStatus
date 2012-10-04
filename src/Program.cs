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
            Console.Title = "Easy to find title!";

            IntPtr hWnd = FindWindow(null, "Easy to find title!");

			// Hide the console
            if (hWnd != IntPtr.Zero)
                ShowWindow(hWnd, 0); 
				
			// The system power charger struct
            SYSTEM_POWER_STATUS status = new SYSTEM_POWER_STATUS(); 

            int current = 0;
            int last = 0;

            while (true)
            {
                System.Threading.Thread.Sleep(100);
				
				// Get Power status from Kernell
                GetSystemPowerStatus(ref status);
				
				// Set current to the current status
                current = status.ACLineStatus; 
				
				// If the state has changed since last check
                if (current != last) 
                {
					// 0 = AC disconnected
					// 1 = AC connected
					switch(current)
					{
						case 0:
							(new SoundPlayer("sound\\doh.wav")).Play();
							break;
						
						case 1:
							(new SoundPlayer("sound\\woohoo.wav")).Play();
							break;
				
						default:
							// Probably not an laptop or there's an error. Just quit
                            System.Windows.Forms.MessageBox.Show("Error! Can't get the status");
							return;
                            break;
					}                        
                }

                last = current; // set last to current
            }
        }
    }
}
