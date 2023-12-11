using EczaneV3.Entites.Models.Order;
using EczaneV3.Entites.Models.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Data.Abstract
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
    }
}
