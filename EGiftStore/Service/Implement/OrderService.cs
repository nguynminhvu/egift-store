using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using Persistence.Enum;
using Persistence.ViewModel.Response;
using Repository;
using Service.Interface;

namespace Service.Implement
{
    public class OrderService : IOrderService
    {
        private IUnitIOfWork _uow;
        private IMapper _mapper;

        public OrderService(IUnitIOfWork unitIOfWork, IMapper mapper)
        {
            _uow = unitIOfWork;
            _mapper = mapper;
        }

        //public async Task<IActionResult> CreateOrder(Guid customerId, CartViewModel cvm)
        //{
        //    Order order = new Order();
        //    double amount = 0;
        //    foreach (var item in cvm.CartItems)
        //    {
        //        amount += item.Quantity * item.Product.Price;
        //    }
        //    order.Amount = amount;
        //    order.CreateDate = DateTime.Now;
        //    order.CustomerId = customerId;
        //    order.Status = "Initialization";
        //    order.Id = Guid.NewGuid();
        //    await _uow.OrderRepository.AddAsync(order);
        //    return await _uow.SaveChangesAsync() > 0 ? new JsonResult(await GetOrderById(order.Id)) : new StatusCodeResult(500);
        //}
        public async Task<IActionResult> CreateOrder(Guid customerId, CartViewModel cvm)
        {
            using (var transaction = _uow.Transaction())
            {
                try
                {
                    double amount = 0;
                    Order order = new Order()
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = customerId,
                        CreateDate = DateTime.Now,
                        Status = OrderStatus.PROCESSING,
                    };
                    foreach (var item in cvm.CartItems)
                    {
                        var product = await _uow.ProductRepository.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                        if (product == null)
                        {
                            return null!;
                        }
                        if (product.Stock < item.Quantity)
                        {
                            return null!;
                        }
                        amount += item.Quantity * item.Product.Price;
                        OrderDetail orderDetail = new OrderDetail()
                        {
                            OrderId = order.Id,
                            CreateDate = DateTime.Now,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                        };
                        await _uow.OrderDetailRepository.AddAsync(orderDetail);
                        product.Stock = product.Stock - item.Quantity;
                    }
                    order.Amount = amount;
                    var cartItem = _uow.CartItemRepository.GetEntitiesPredicate(x => x.CartId.Equals(cvm.Id));

                    _uow.CartItemRepository.RemoveRange(cartItem);
                    await _uow.OrderRepository.AddAsync(order);
                    await _uow.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new JsonResult(await GetOrderById(order.Id));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    await transaction.DisposeAsync();
                }
            }
        }


        public async Task<OrderViewModel> GetOrderById(Guid id)
        {
            var order = await _uow.OrderRepository.FirstOrDefaultAsync(x => x.Id.Equals(id), x => x.OrderDetails);
            return _mapper.Map<OrderViewModel>(order);
        }

        public async Task<List<OrderViewModel>> GetOrders(Guid customerId)
        {
            var orders = await _uow.OrderRepository.GetEntitiesPredicate(x => x.CustomerId.Equals(customerId) && x.Status != "Done" && x.Status != "Cancel", x => x.OrderDetails).ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            return orders.Count > 0 ? orders : null!;
        }

        public async Task<IActionResult> UpdateOrder(Guid orderId, string status)
        {
            var order = await _uow.OrderRepository.FirstOrDefaultAsync(x => x.Id.Equals(orderId) && x.Status != "Cancel");
            if (order != null)
            {
                order.Status = status;
                return await _uow.SaveChangesAsync() > 0 ? new JsonResult(await GetOrderById(orderId)) : new StatusCodeResult(500);
            }
            return new StatusCodeResult(404);
        }
    }
}
