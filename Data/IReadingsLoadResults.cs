namespace Data
{
    public interface IReadingsLoadResults
    {
        public int Loaded { get; } 
        public int Rejected { get; }
    }
}
