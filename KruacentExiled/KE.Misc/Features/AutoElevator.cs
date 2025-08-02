using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Misc.Features
{
    /// <summary>
    /// The elevator will random activate in the round
    /// </summary>
    internal class AutoElevator
    {
        private CoroutineHandle handle;
        ~AutoElevator()
        {
            Timing.KillCoroutines(handle);
        }


        public void StartLoop()
        {
            handle = Timing.RunCoroutine(StartElevator());
        }
        /// <summary>
        /// Start the auto elevator loop
        /// </summary>
        private IEnumerator<float> StartElevator()
        {
            Log.Debug("elevator");
            while (!Round.IsEnded)
            {
                foreach (Lift l in Lift.List)
                {
                    yield return Timing.WaitForSeconds(UnityEngine.Random.Range(30, 45));
                    SendElevator(l);
                }
            }
            /*
            [2025-07-28 22:12:45.541 +02:00] [STDOUT] InvalidOperationException: Collection was modified; enumeration operation may not execute.
            [2025-07-28 22:12:45.541 +02:00] [STDOUT]   at System.Collections.Generic.Dictionary`2+ValueCollection+Enumerator[TKey,TValue].MoveNext () [0x00013] in <069d7b80a3914a08b6825aa362b07f5e>:0
            [2025-07-28 22:12:45.541 +02:00] [STDOUT]   at KE.Misc.Features.AutoElevator+<StartElevator>d__0.MoveNext () [0x00098] in <cc390f46e4a44c83b359884eb056d955>:0
            [2025-07-28 22:12:45.541 +02:00] [STDOUT]   at MEC.Timing.Update () [0x0043c] in <907db9b8bd144382918df78d2897b4b7>:0
            [2025-07-28 22:12:45.541 +02:00] [STDOUT] UnityEngine.DebugLogHandler:Internal_LogException_Injected(Exception, IntPtr)
            [2025-07-28 22:12:45.541 +02:00] [STDOUT] UnityEngine.DebugLogHandler:Internal_LogException(Exception, Object)
            [2025-07-28 22:12:45.541 +02:00] [STDOUT] UnityEngine.DebugLogHandler:LogException(Exception, Object)
            [2025-07-28 22:12:45.541 +02:00] [STDOUT] UnityEngine.Logger:LogException(Exception, Object)
            [2025-07-28 22:12:45.541 +02:00] [STDOUT] UnityEngine.Debug:LogException(Exception)
            [2025-07-28 22:12:45.541 +02:00] [STDOUT] MEC.Timing:Update()
            [2025-07-28 22:12:49.976 +02:00] [STDOUT] ArgumentNullException: Value cannot be null.
            [2025-07-28 22:12:49.976 +02:00] [STDOUT] Parameter name: key
            [2025-07-28 22:12:49.976 +02:00] [STDOUT]   at System.Collections.Concurrent.ConcurrentDictionary`2[TKey,TValue].ThrowKeyNullException () [0x00000] in <069d7b80a3914a08b6825aa362b07f5e>:0
            [2025-07-28 22:12:49.976 +02:00] [STDOUT]   at System.Collections.Concurrent.ConcurrentDictionary`2[TKey,TValue].TryRemove (TKey key, TValue& value) [0x00008] in <069d7b80a3914a08b6825aa362b07f5e>:0
            [2025-07-28 22:12:49.976 +02:00] [STDOUT]   at RemoteAdmin.QueryProcessor.OnDestroy () [0x00018] in <d9731e675e55453197cf28cd60eed3f2>:0
            */
        }


        private void SendElevator(Lift e)
        {
            Log.Debug($"{e.Name}");
            e.TryStart(0, true);
        }
    }
}
