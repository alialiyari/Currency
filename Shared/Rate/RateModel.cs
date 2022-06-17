namespace Rate
{
    public class InfoModel
    {
        public long Id { get; set; }
        public string To { get; set; }
        public string From { get; set; }



        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }


        public double Rate { get; set; }
    }
    public class SaveModel
    {
        public string To { get; set; }
        public string From { get; set; }
        public double Rate { get; set; }
    }


    public class NodeModel
    {
        public double Rate { get; set; }
        public string Currency { get; set; }
        public NodeModel ParentNode { get; set; }
    }
}
