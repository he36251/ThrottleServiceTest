using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ThrottleServiceTest
{
    public static class ThrottlerManager
    {
        private static ConcurrentDictionary<string, Throttler> _throttlers = new ConcurrentDictionary<string, Throttler>();

        public static void CheckThrottlers()
        {
            if (_throttlers.IsEmpty)
            {
                return;
            }

            foreach (KeyValuePair<string, Throttler> throttler in _throttlers)
            {
                if (throttler.Value.CheckQueue())
                {
                    _throttlers.TryRemove(throttler.Value.Id, out _);
                    Console.WriteLine($"Throttler: {throttler.Value.Id} has ended!");
                }
            }
        }

        public static void AddItem(QueueItem queueItem)
        {
            //Search for corresponding throttler
            if (_throttlers.TryGetValue(queueItem.ParentId, out Throttler throttler))
            {
                throttler.AddItem(queueItem);
            }
            else
            {
                throttler = new Throttler(queueItem.ParentId);
                throttler.AddItem(queueItem);
                _throttlers.TryAdd(queueItem.ParentId, throttler);
            }
        }
    }
}