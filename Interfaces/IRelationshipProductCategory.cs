namespace Composition.Interfaces
{
    public interface IRelationshipProductCategory
    {
        /// <summary>
        /// Create a relationship of product/category
        /// </summary>
        /// <param name="productId">non null guid product id</param>
        /// <param name="categoryId">non null guid category id</param>
        /// <returns>false/true</returns>
        public Task<bool> CreateRelationship(Guid productId, Guid categoryId);
        /// <summary>
        /// Delete a relationship of product/category
        /// </summary>
        /// <param name="productId">non null guid product id</param>
        /// <param name="categoryId">non null guid category id</param>
        /// <returns></returns>
        public Task<bool> DeleteRelationship(Guid productId, Guid categoryId);
        /// <summary>
        /// Updates relationship obj
        /// </summary>
        /// <param name="categoryIds">non null List of category ids</param>
        /// <param name="productId">non null guid productid</param>
        /// <returns>false/true</returns>
        public Task<bool> UpdateRelationship(List<Guid> categoryIds, Guid productId);
    }
}
