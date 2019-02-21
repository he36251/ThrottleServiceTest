using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ThrottleServiceTest;

namespace Testing
{
    class Program
    {
        private static int _itemsCount = 1;

        static void Main(string[] args)
        {
            ThrottleService throttleService = new ThrottleService();
            List<QueueItem> testList = new List<QueueItem>();

            TestAddItems(testList, "111", 10, QueueItemTypes.Comment);
            TestAddItems(testList, "333", 5, QueueItemTypes.Idea);
            TestAddItems(testList, "222", 6, QueueItemTypes.Comment);
            TestAddItems(testList, "111", 6, QueueItemTypes.Comment);

            foreach (QueueItem queueItem in testList)
            {
                throttleService.AddItem(queueItem);
            }

            //Tasks
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                Task.Factory.StartNew(ThrottlerManager.CheckThrottlers, cancellationTokenSource.Token);

                Thread.Sleep(1000);
            }
        }

        private static void TestAddItems(List<QueueItem> list, string parentId, int itemCount, QueueItemTypes queueItemTypes)
        {
            for (int i = 0; i < itemCount; i++)
            {
                list.Add(new QueueItem() {ParentId = parentId, Content = "content", Type = queueItemTypes});
            }
        }
    }
}