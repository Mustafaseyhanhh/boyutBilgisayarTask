using EczaneV3.Data.Abstract;
using EczaneV3.Data.Contexts;
using EczaneV3.Entites.Models.Order;
using EczaneV3.Entites.Models.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Data.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(EczaneV3DbContext eczaneV3DbContext) : base(eczaneV3DbContext)
        {
        }
    }
}
