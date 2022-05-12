﻿using Domain.Entities;
using Application.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IOrderItemRepository : IBaseRepository<OrderItem>
    {
        List<OrderItem> CreateOrderItems(List<Inventory> inventories, int quantity);
    }
}
