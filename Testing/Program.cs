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

//            List<QueueItem> testList = new List<QueueItem>
//            {
//                new QueueItem() {Id = 1, ParentId = "222", Content = "first", Type = QueueItemTypes.Comment},
//                new QueueItem() {Id = 2, ParentId = "444", Content = "second", Type = QueueItemTypes.Idea},
//                new QueueItem() {Id = 3, ParentId = "222", Content = "third", Type = QueueItemTypes.Comment},
//                new QueueItem() {Id = 4, ParentId = "222", Content = "fourth", Type = QueueItemTypes.Comment},
//                new QueueItem() {Id = 5, ParentId = "333", Content = "fifth", Type = QueueItemTypes.Comment},
//                new QueueItem() {Id = 6, ParentId = "444", Content = "sixth", Type = QueueItemTypes.Idea}
//            };

            List<QueueItem> testList = new List<QueueItem>();

            TestAddItems(testList, "111", 10, QueueItemTypes.Comment);
            TestAddItems(testList, "222", 6, QueueItemTypes.Comment);
            TestAddItems(testList, "333", 5, QueueItemTypes.Idea);
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

                Thread.Sleep(500);
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