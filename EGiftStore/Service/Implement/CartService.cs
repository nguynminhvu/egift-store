using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using Persistence.ViewModel.Request;
using Persistence.ViewModel.Response;
using Repository;
using Service.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implement
{
	public class CartService : ICartService
	{
		private IUnitIOfWork _uow;
		private IMapper _mapper;

		public CartService(IUnitIOfWork unitIOfWork, IMapper mapper)
		{
			_uow = unitIOfWork;
			_mapper = mapper;
		}

		public async Task<IActionResult> AddToCart(Guid customerId, AddToCartModel atc)
		{
			var product = await _uow.ProductRepository.FirstOrDefaultAsync(x => x.Id.Equals(atc.ProductId)) as Product;
			if (product is null || product.Stock < 1)
			{
				return new StatusCodeResult(400);
			}
			Cart cartCheck = await _uow.CartRepository.FirstOrDefaultAsync(c => c.CustomerId.Equals(customerId), x => x.CartItems);
			if (cartCheck == null)
			{
				Cart cart = new Cart
				{
					Id = Guid.NewGuid(),
					CustomerId = customerId,
				};
				await _uow.CartRepository.AddAsync(cart);
				CartItem cartItem = new CartItem
				{
					CartId = cart.Id,
					ProductId = atc.ProductId,
					Quantity = atc.Quantity,
					CreateDate = DateTime.Now
				};
				await _uow.CartItemRepository.AddAsync(cartItem);
				return await _uow.SaveChangesAsync() > 0 ? new JsonResult((await GetCartItems(customerId) as JsonResult)!.Value) : new StatusCodeResult(500);
			}
			else
			{
				CartItem cartItem = new CartItem { CartId = cartCheck.Id, ProductId = atc.ProductId, Quantity = atc.Quantity, CreateDate = DateTime.Now };
				if (cartCheck.CartItems == null)
				{
					await _uow.CartItemRepository.AddAsync(cartItem);
				}
				else
				{
					foreach (var item in cartCheck.CartItems)
					{
						if (item.ProductId.Equals(cartItem.ProductId))
						{
							cartItem.Quantity += item.Quantity;
							_uow.CartItemRepository.Remove(item);
						}
					}
					await _uow.CartItemRepository.AddAsync(cartItem);
					return await _uow.SaveChangesAsync() > 0 ? new JsonResult((await GetCartItems(customerId) as JsonResult)!.Value) : new StatusCodeResult(500);
				}
			}
			return new StatusCodeResult(400);
		}

		public async Task<IActionResult> GetCartItems(Guid customerId)
		{
			var cart = await _uow.CartRepository.FirstOrDefaultAsync(x => x.CustomerId.Equals(customerId), x => x.CartItems);
			if (cart == null)
			{
				cart = new Cart
				{
					Id = Guid.NewGuid(),
					CustomerId = customerId
				};
				await _uow.CartRepository.AddAsync(cart);
				await _uow.SaveChangesAsync();
			}
			var cartItem = await _uow.CartItemRepository.GetEntitiesPredicate(x => x.CartId.Equals(cart.Id)).ProjectTo<CartItemViewModel>(_mapper.ConfigurationProvider).ToListAsync();
			var cartViewModel = _mapper.Map<CartViewModel>(cart);
			cartViewModel.CartItems = cartItem;
			return new JsonResult(cartViewModel);
		}

		public async Task<IActionResult> UpdateCart(CartUpdateModel cum)
		{
			List<Guid> productIds = new List<Guid>();
			foreach (var item in cum.CartItems)
			{
				productIds.Add(item.ProductId);
			}
			var products = await _uow.ProductRepository.GetEntitiesPredicate(x => productIds.Contains(x.Id)).ToListAsync();
			productIds.Clear();
			foreach (var item in products)
			{
				productIds.Add(item.Id);
			}
			var cart = await _uow.CartRepository.FirstOrDefaultAsync(x => x.Id.Equals(cum.CartId), x => x.CartItems);
			_uow.CartItemRepository.RemoveRange(cart.CartItems);
			foreach (var item in cum.CartItems)
			{
				if (productIds.Contains(item.ProductId))
				{
					cart.CartItems.Add(new CartItem
					{
						CartId = cart.Id,
						ProductId = item.ProductId,
						Quantity = item.Quantity,
						CreateDate = DateTime.Now
					});
				}
			}
			return await _uow.SaveChangesAsync() > 0 ? new JsonResult((await GetCartItems(cart.CustomerId) as JsonResult)!.Value) : new StatusCodeResult(500);
		}


	}
}
