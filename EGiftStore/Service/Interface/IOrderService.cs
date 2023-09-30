using Microsoft.AspNetCore.Mvc;
using Persistence.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IOrderService
    {
        Task<IActionResult> CreateOrder(Guid customerId, CartViewModel cvm);
        Task<IActionResult> UpdateOrder(Guid orderId, string status);
        Task<OrderViewModel> GetOrderById(Guid id);
        Task<List<OrderViewModel>> GetOrders(Guid customerId);
       //Task<IActionResult> CreateOrderTransaction(Guid customerId, CartViewModel cvm);

    }
}
