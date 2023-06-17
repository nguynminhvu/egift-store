using AutoMapper;
using Persistence.Entities;
using Persistence.ViewModel.Response;
using Repository;
using Service.Interface;

namespace Service.Implement
{
	public class OrderDetailService : IOrderDetailService
	{
		private IUnitIOfWork _uow;
		private IMapper _mapper;

		public OrderDetailService(IUnitIOfWork unitIOfWork, IMapper mapper)
		{
			_uow = unitIOfWork;
			_mapper = mapper;
		}

		public async Task<bool> CreateOrderDetail(Guid orderId, CartViewModel cartViewModel)
		{
			foreach (var item in cartViewModel.CartItems)
			{
				OrderDetail orderDetail = new OrderDetail
				{
					OrderId = orderId,
					CreateDate = DateTime.Now,
					Quantity = item.Quantity,
					ProductId = item.ProductId,
				};
				await _uow.OrderDetailRepository.AddAsync(orderDetail);
			}
			return await _uow.SaveChangesAsync() > 0 ? true : false;
		}
	}
}
