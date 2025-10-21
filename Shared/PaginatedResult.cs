using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class PaginatedResult<TEntity>
    {
        public PaginatedResult(int _pagesize,int _pageindex,int _count,IEnumerable<TEntity>_data)
        { 
            PageSize = _pageindex;
            PageIndex = _pagesize;
            count = _count;
            data = _data;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int count { get; set; }
        public IEnumerable<TEntity> data { get; set; }
    }
}
