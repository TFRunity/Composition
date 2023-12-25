using Composition.Models;
using Composition.ViewModels;
namespace Composition.Interfaces
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Creates order by it's viewmodel
        /// </summary>
        /// <param name="viewModel">nullable orderviewmodel</param>
        /// <returns>false/true</returns>
        public Task<bool> Create(CreateOrderItemViewModel? viewModel);
        /// <summary>
        /// Updating order by it's updated ver
        /// </summary>
        /// <param name="updatedOrder">nullable updated order</param>
        /// <returns>false/true</returns>
        public Task<bool> Update(Order? updatedOrder);
        /// <summary>
        /// Confirms that order is paid
        /// </summary>
        /// <param name="OrderId">nullable guid orderid</param>
        /// <param name="Email">nullable string</param>
        /// <returns></returns>
        public Task<bool> ConfirmOrderByIdAndEmail(Guid? orderId, string? email);
        /// <summary>
        /// Deleting order by it's id (cascade)
        /// </summary>
        /// <param name="orderId">nullable orderid guid</param>
        /// <returns>false/true</returns>
        public Task<bool> DeleteById(Guid? orderId);
        /// <summary>
        /// Deleting orderitem from order by it's id
        /// </summary>
        /// <param name="productId">nullable orderitemId guid</param>
        /// <returns>false/true</returns>
        public Task<bool> DeleteObjFromOrder(Guid? orderitemId);
        /// <summary>
        /// Clears currentorder and reseting it's price
        /// </summary>
        /// <param name="orderId">nullable orderId guid</param>
        /// <returns>false/true</returns>
        public Task<bool> Clear(Guid? orderId);
        /// <summary>
        /// Get order by it's id
        /// </summary>
        /// <param name="orderId">nullable orderid guid</param>
        /// <returns>order/null</returns>
        public Task<Order?> GetById(Guid? orderId);
        /// <summary>
        /// Get all orders (moderator only)
        /// </summary>
        /// <returns>List Order/null</returns>
        public Task<List<Order>?> GetAll();
        /// <summary>
        /// Changes count of this orderitem
        /// </summary>
        /// <param name="viewModel">nullable counterviewmodel </param>
        /// <returns>false/true</returns>
        public Task<bool> ChangeCount(CounterViewModel? viewModel);
    }
}
