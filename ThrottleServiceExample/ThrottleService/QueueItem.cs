namespace ThrottleServiceTest
{
    public class QueueItem
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public QueueItemTypes Type { get; set; }
        
        public string ParentId { get; set; }

        public string StrValue
        {
            get
            {
                string typeString = "";
                
                switch (Type)
                {
                    case QueueItemTypes.Idea:
                        typeString = "IDEA";
                        break;

                    case QueueItemTypes.Comment:
                        typeString = "COMMENT";
                        break;
                }

                return $"{typeString} -- {ParentId}: {Content}";
            }
        }
    }
}