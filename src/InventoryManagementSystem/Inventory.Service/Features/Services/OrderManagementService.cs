using Inventory.DataAccess.Entites;
using Inventory.DataAccess.Enums;
using Inventory.DataAccess.IdentityManager;
using Inventory.DataAccess.UnitOfWork;
using Inventory.Service.Features.Services.IServices;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Features.Services
{
    public class OrderManagementService : IOrderManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductManagementService _productManagementService;
        public OrderManagementService(IUnitOfWork unitOfWork, IProductManagementService productManagementService)
        {
            _unitOfWork = unitOfWork;
            _productManagementService = productManagementService;
        }


        public async Task<IEnumerable<Order>> GetAllOrder()
        {
            return await _unitOfWork.Order.GetAllAsync(includeProperties: "Product");
        }


        public async Task<IEnumerable<Order>> GetAllSaleOrder(Expression<Func<Order, bool>> filter = null)
        {
            return await _unitOfWork.Order.GetAllAsync(x => x.OrderType == OrderType.Sale.ToString(), includeProperties: "Product");

        }

        public async Task<IEnumerable<Order>> GetAllPurchaseOrder(Expression<Func<Order, bool>> filter = null)
        {
            return await _unitOfWork.Order.GetAllAsync(x => x.OrderType == OrderType.Purchase.ToString(), includeProperties: "Product");

        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAndType(DateTime searchFrom, DateTime searchTo, string orderType)
        {
            var orders = await _unitOfWork.Order.GetAllAsync(x =>
                x.OrderType == orderType &&
                x.CreatedDate.Date >= searchFrom.Date &&
                x.CreatedDate.Date <= searchTo.Date,
                includeProperties: "Product");

            return orders;
        }

        public async Task CreateOrderAsync(Guid userId, Guid productId, int totoalQuantity, decimal unitPrice, decimal totalAmount, string orderType)
        {

            var product = await _productManagementService.GetProductByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found");



            if (orderType == OrderType.Purchase.ToString())
            {
                product.QuantityInStock += totoalQuantity;

                if (product.Price < unitPrice)
                {
                    product.Price = unitPrice;
                }

                await _unitOfWork.Product.UpdateAsync(product);
            }


            if (orderType == OrderType.Sale.ToString())
            {
                if (product.QuantityInStock < totoalQuantity)
                    throw new Exception("Not enough product.");

                product.QuantityInStock -= totoalQuantity;

                await _unitOfWork.Product.UpdateAsync(product);
            }

            var order = new Order
            {
                ProductId = productId,
                TotalQuantity = totoalQuantity,
                TotalAmount = totalAmount,
                UnitPrice = unitPrice,
                OrderType = orderType,
                CreatedDate = DateTime.Now,
                Product = product
            };

            await _unitOfWork.Order.CreateAsync(order);

        }


        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await _unitOfWork.Order.GetAsync(x => x.Id == id, includeProperties: "Product");
        }

        public async Task RemoveOrderAsync(Order order)
        {
            var product = await _productManagementService.GetProductByIdAsync(order.ProductId);

            if (order.OrderType == OrderType.Sale.ToString())
            {
                if (product != null)
                    product.QuantityInStock += order.TotalQuantity;

            }
            else
            {
                if(product != null && product.QuantityInStock < order.TotalQuantity)
                {
                    throw new Exception("Cannot edit this purchase order due to product shortage");
                }
                else
                {
                    product.QuantityInStock -= order.TotalQuantity;
                }
            }
            

            await _unitOfWork.Product.UpdateAsync(product);

            await _unitOfWork.Order.RemoveAsync(order);
        }


        public async Task<IEnumerable<Order>> GetOrderByUserIdAsync(Guid userId)
        {
            return await _unitOfWork.Order.GetAllAsync(x => x.CustomerId == userId, includeProperties: "User,Product");
        }



        public async Task<IEnumerable<Product>> GetAllProductNameAsync()
        {
            return await _productManagementService.GetAllProducts();
        }

        public async Task UpdateOrderAsync(Guid orderId, Guid productId, int totoalQuantity, decimal unitPrice, decimal totalAmount, string orderType)
        {

            var order = await GetOrderByIdAsync(orderId);
            if (order == null)
                throw new Exception("Order not found");

            var product = await _productManagementService.GetProductByIdAsync(productId);


            //purchase

            if (orderType == OrderType.Purchase.ToString())
            {
                if (order.TotalQuantity < totoalQuantity)
                {
                    var diff = totoalQuantity - order.TotalQuantity;

                    order.TotalQuantity += diff;

                    if (order.UnitPrice < unitPrice)
                    {
                        order.UnitPrice = unitPrice;
                        product.Price = unitPrice;
                    }

                    product.QuantityInStock += diff;

                    await _unitOfWork.Product.UpdateAsync(product);

                    order.TotalAmount = totalAmount;
                    await _unitOfWork.Order.UpdateAsync(order);

                }
                else if ((order.TotalQuantity - totoalQuantity) <= product.QuantityInStock)
                {
                    var diff = order.TotalQuantity - totoalQuantity;

                    order.TotalQuantity -= diff;

                    if (order.UnitPrice < unitPrice)
                    {
                        order.UnitPrice = unitPrice;
                        product.Price = unitPrice;
                    }

                    product.QuantityInStock -= diff;

                    await _unitOfWork.Product.UpdateAsync(product);

                    order.TotalAmount = totalAmount;
                    await _unitOfWork.Order.UpdateAsync(order);
                }
                else
                {
                    throw new Exception("Cannot edit this purchase order due to product shortage");
                }

            }


            //sale 

            if (orderType == OrderType.Sale.ToString())
            {
                if (order.TotalQuantity > totoalQuantity)
                {
                    var diff = order.TotalQuantity - totoalQuantity;

                    order.TotalQuantity -= diff;
                    product.QuantityInStock += diff;
                    order.UpdatedDate = DateTime.Now;

                    await _unitOfWork.Product.UpdateAsync(product);

                    order.TotalAmount = totalAmount;
                    await _unitOfWork.Order.UpdateAsync(order);
                }
                else if (order.TotalQuantity <= totoalQuantity)
                {
                    if ((totoalQuantity - order.TotalQuantity) <= product.QuantityInStock)
                    {
                        var diff = totoalQuantity - order.TotalQuantity;

                        order.TotalQuantity += diff;
                        product.QuantityInStock -= diff;

                        await _unitOfWork.Product.UpdateAsync(product);

                        order.TotalAmount = totalAmount;
                        order.UpdatedDate = DateTime.Now;
                        await _unitOfWork.Order.UpdateAsync(order);
                    }
                    else
                    {
                        throw new Exception("Cannot edit this sale order due to product shortage");

                    }
                }

            }



        }

        public async Task<int> GetAllPurchaseOrderCount(Expression<Func<Order, bool>> filter = null)
        {
            var orders = await GetAllPurchaseOrder();



            if (filter != null)
            {
                IQueryable<Order> queryableOrders = orders.AsQueryable();
                queryableOrders = queryableOrders.Where(filter);
                return queryableOrders.Count();
            }
            return orders.Count();
        }

        public async Task<int> GetAllSaleOrderCount(Expression<Func<Order, bool>> filter = null)
        {
            var orders = await GetAllSaleOrder();


            if (filter != null)
            {
                IQueryable<Order> queryableOrders = orders.AsQueryable();
                queryableOrders = queryableOrders.Where(filter);
                return queryableOrders.Count();
            }
            return orders.Count();
        }

        public async Task<Product> GetProductAsync(Guid productId)
        {
            return await _productManagementService.GetProductByIdAsync(productId);
        }
    }
}
