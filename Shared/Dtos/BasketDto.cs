using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class BasketDto
    {
        public string Id { get; set; } = string.Empty;
        public List<BasketItemDto> Items { get; set; } 
    }
}
