using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.UC.MasterData
{
    public class PagingInfo
    {
        public List<PagingRow> Items { get; set; }
        public PagingInfo (int totalPages)
        {
            Items = new List<PagingRow>();
            for (int i = 1; i<=totalPages; i++)
            {
                Items.Add(new PagingRow()
                {
                    Page = i,
                    TotalPages = totalPages
                });
            }
        }
    }
}
