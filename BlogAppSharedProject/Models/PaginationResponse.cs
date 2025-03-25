using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAppSharedProject.Models
{
    public class PaginationResponse<T>
    {
        public int TotalRecords { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
