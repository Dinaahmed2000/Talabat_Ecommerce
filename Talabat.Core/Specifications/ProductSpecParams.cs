namespace Talabat.Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 10;
        private int pagesize=5;

        public int pageSize
        {
            get { return pagesize; }
            set { pagesize = value > MaxPageSize ? MaxPageSize : value ; }
        }

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }

        public int pageIndex { get; set; } = 1;
        public string? sort { get; set; }
        public int? brandId { get; set; }
        public int? typeId { get; set; }
    }
}
