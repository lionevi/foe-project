using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Foe.Server;

namespace Foe.Server
{
    public delegate void TimerFunction(string[] args);
    public delegate bool WakeFunction(); // Function that will wake the Sleep function.

    public static class FoeServerScheduler
    {
        public static void Timer(TimerFunction func, int seconds, string registryName)
        {
            // seconds must >= 1
            if (seconds < 1)
            {
                throw new Exception("The \"seconds\" argument in FoeServerScheduler.Timer must be >= 1.");
            }

            int timeRemain = 0;

            try
            {
                // Reset registry flag
                ResetRegistryFlag();

                while (!IsStopProcess())
                {
                    if (timeRemain <= 0)
                    {
                        // Call function
                        func(null);

                        // reset timer
                        timeRemain = seconds;
                    }
                    else
                    {
                        timeRemain--;
                    }

                    Thread.Sleep(1000);
                }

            }
            catch (Exception except)
            {
                throw except;
            }
        }

        private static void ResetRegistryFlag()
        {
            // Reset the EndRegistrationProcessor to false.
            // When it's true, this process will stop, so we need to make sure it is set to false in the beginning.
            FoeServerRegistry.Set("EndFoeRssFeedUpdater", "F");
        }

        private static bool IsStopProcess()
        {
            string value = FoeServerRegistry.Get("EndFoeRssFeedUpdater");
            if ((value != null) && (value.ToUpper().CompareTo("T") == 0))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sleep for "duration" number of seconds. However, if during the sleep that the wakeupCall fuction returns true, then Sleep will exit immediately.
        /// </summary>
        /// <param name="wakeupCall"></param>
        /// <param name="duration"></param>
        public static void Sleep(WakeFunction WakeupCall, int duration)
        {
            for (int i = 0; i < duration; i++)
            {
                if (WakeupCall())
                {
                    break;
                }

                Thread.Sleep(1000);
            }
        }
    }
}
