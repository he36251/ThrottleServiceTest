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

            foreach (var (key, throttler) in _throttlers)
            {
                if (!throttler.CheckQueue())
                {
                    _throttlers.TryRemove(key, out _);
                    Console.WriteLine($"Throttler: {key} has ended!");
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