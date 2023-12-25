using Composition.Interfaces;
using Composition.Models;
using Composition.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Composition.Database.Methods
{
    public class OrderMethods : IOrderRepository
    {
        private readonly AppDBContext appDBContext;
        private readonly IUserRepository userMethods;
        private readonly ISubCategoryRepository subCategoryMethods;

        public OrderMethods(ISubCategoryRepository _subCategoryMethods, AppDBContext _appDBContext, IUserRepository _userRepository)
        {
            subCategoryMethods = _subCategoryMethods;
            appDBContext = _appDBContext;
            userMethods = _userRepository;
        }
        public async Task<bool> ChangeCount(CounterViewModel? viewModel)
        {
            if (viewModel == null)
                return false;
            OrderItem? orderItem = await appDBContext.OrderItems.FindAsync(viewModel.OrderItemId);
            if (orderItem == null)
                return false;
            Order? order = orderItem.Order;
            if (order == null)
                return false;
            order.TotalPrice -= orderItem.PriceFromProduct * viewModel.PreviousCount;
            order.TotalPrice += orderItem.PriceFromProduct * viewModel.CurrentCount;
            orderItem.CurrentCount = viewModel.CurrentCount;
            orderItem.PreviousCount = viewModel.CurrentCount;
            await appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Clear(Guid? orderId)
        {
            if (orderId == null || orderId == Guid.Empty)
                return false;
            Order? order = await GetById(orderId);
            if (order == null)
                return false;
            appDBContext.OrderItems.RemoveRange(order.OrderItems);
            order.TotalPrice = 0;
            await appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmOrderByIdAndEmail(Guid? orderId, string? email)
        {
            if (orderId == null || orderId == Guid.Empty)
                return false;
            Order? order = await GetById(orderId);
            if (order == null)
                return false;
            order.Paid = true;
            User? user = await userMethods.GetByEmail(email);
            await userMethods.ResetOrderState(user);
            return true;
        }

        public async Task<bool> Create(CreateOrderItemViewModel? viewModel)
        {
            if (viewModel == null)
                return false;
            User? user = await userMethods.GetByEmail(viewModel.Email);
            if (user == null)
                return false;
            if (user.CurrentOrderId == Guid.Empty)
            {
                bool result = await CreateNewOrder(viewModel);
                if (result == false)
                    return false;
            }
            else
            {
                bool result = await AddItemToOrder(viewModel);
                if (result == false)
                    return false;
            }
            return true;
        }

        private async Task<bool> CreateNewOrder(CreateOrderItemViewModel viewModel)
        {
            SubCategory? subCategory = await subCategoryMethods.GetCurrent(viewModel.SubCategoryId);
            if (subCategory == null)
                return false;
            Order order = new Order();
            OrderItem item = new OrderItem() { IdFromProduct = viewModel.ProductId, PriceFromProduct = subCategory.Price, IdFromSubCategory = viewModel.SubCategoryId, _Case = viewModel._Case };
            await appDBContext.OrderItems.AddAsync(item);
            order.OrderItems.Add(item);
            order.TotalPrice += item.PriceFromProduct * item.CurrentCount;
            await appDBContext.Orders.AddAsync(order);
            bool result = await userMethods.AddNewOrder(order, viewModel.Email);
            if (result == false)
                return false;
            return true;
        }
        private async Task<bool> AddItemToOrder(CreateOrderItemViewModel viewModel)
        {
            User? user = await userMethods.GetByEmail(viewModel.Email);
            if (user == null)
                return false;
            SubCategory? subCategory = await subCategoryMethods.GetCurrent(viewModel.SubCategoryId);
            if (subCategory == null)
                return false;
            Order? order = await GetById(user.CurrentOrderId);
            if(order == null)
                return false;
            OrderItem item = new OrderItem() { IdFromProduct = viewModel.ProductId, PriceFromProduct = subCategory.Price, IdFromSubCategory = viewModel.SubCategoryId, _Case = viewModel._Case };
            await appDBContext.OrderItems.AddAsync(item);
            order.OrderItems.Add(item);
            order.TotalPrice += item.PriceFromProduct * item.CurrentCount;
            await appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteById(Guid? orderId)
        {
            if (orderId == null || orderId == Guid.Empty)
                return false;
            Order? order = await appDBContext.Orders.FindAsync(orderId);
            if (order == null)
                return false;
            appDBContext.Orders.Remove(order);
            await appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteObjFromOrder(Guid? orderItemId)
        {
            if (orderItemId == null || orderItemId == Guid.Empty)
                return false;
            OrderItem? item = await appDBContext.OrderItems.FindAsync(orderItemId);
            if (item == null)
                return false;
            Order? order = item.Order;
            if (order == null)
                return false;
            order.TotalPrice -= item.PriceFromProduct * item.CurrentCount;
            appDBContext.OrderItems.Remove(item);
            await appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Order>?> GetAll()
        {
            List<Order> orders = await appDBContext.Orders
                .Include(a => a.OrderItems)
                .Include(b => b.User)
                .ToListAsync();
            return orders;
        }

        public async Task<Order?> GetById(Guid? orderId)
        {
            if (orderId == null || orderId == Guid.Empty)
                return null;
            Order? order = await appDBContext.Orders.FindAsync(orderId);
            return order == null ? null : order;
        }

        public async Task<bool> Update(Order? updatedOrder)
        {
            if (updatedOrder == null)
                return false;
            Order? order = await GetById(updatedOrder.Id);
            if (order == null)
                return false;
            order.Done = updatedOrder.Done;
            await appDBContext.SaveChangesAsync();
            return true;
        }
    }
}
