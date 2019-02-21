using System;

namespace ThrottleServiceTest
{
    public class ThrottleService
       {
           public void AddItem(QueueItem item)
           {
               ThrottlerManager.AddItem(item);
           }   
       }
   }