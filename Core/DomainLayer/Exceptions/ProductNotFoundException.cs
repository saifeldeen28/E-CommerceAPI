using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public class ProductNotFoundException(int id): NotFoundException($"The Product with id {id} was not found.")    
    {
    }
}
