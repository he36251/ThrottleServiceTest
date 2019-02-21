using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace ThrottleServiceTest
{
    public class Throttler
    {
        public readonly string Id;
        private Queue<QueueItem> ItemsQueue { get; }
        private Stopwatch Stopwatch { get; }

        //Config this...
        private int ItemCount { get; set; } = 0;

        public Throttler(string id)
        {
            Id = id;
            ItemsQueue = new Queue<QueueItem>();
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
            
            Console.WriteLine($"Throttler: {Id} has started!");
        }

        public bool CheckQueue()
        {
            Console.WriteLine($"Checking: {Id}, Stopwatch: {Stopwatch.Elapsed.Seconds}s, Delay: {ThrottleServiceConstant.DelayMillisec - CompensationTime()}");
            
            if (Stopwatch.ElapsedMilliseconds < (ThrottleServiceConstant.DelayMillisec - CompensationTime()))
            {
                return true;
            }

            //Return everything from the queue
            ProcessQueue();

            ItemCount = 0;
            Stopwatch.Reset();

            return false;
        }

        private double CompensationTime()
        {
            if (ItemCount < ThrottleServiceConstant.ThrottleThreshold)
            {
                return 0;
            }

            double initialDelayPenalty = ThrottleServiceConstant.DelayMillisec / 10;

            //Stops having items fired immediately
            if (ItemCount >= 10)
            {
                return 9 * initialDelayPenalty;
            }
            
            return ItemCount * initialDelayPenalty;
        }

        public void AddItem(QueueItem item)
        {
//            Console.WriteLine($"Item added: {item.Type}");
            
            //Deal with first 2 items immediately
            if (ItemCount < ThrottleServiceConstant.ThrottleThreshold)
            {
                ProcessItem(item);
                ItemCount++;
                return;
            }
            
            ItemsQueue.Enqueue(item);
            ItemCount++;
            
            Stopwatch.Restart();
        }

        //Do everything at once
        private void ProcessQueue()
        {
            string printStr = "----QUEUE DUMP-----\n";

            while (ItemsQueue.Count > 0)
            {
                printStr += $"{ItemsQueue.Dequeue().StrValue}\n";
            }

            printStr += "----------------";
            
            Console.WriteLine(printStr);
        }
        
        private void ProcessItem(QueueItem item)
        {
            //Deal with different types
            switch (item.Type)
            {
                case QueueItemTypes.Idea:
                    Console.WriteLine(item.StrValue);
                    break;

                case QueueItemTypes.Comment:
                    Console.WriteLine(item.StrValue);
                    break;
            }
        }
    }
}