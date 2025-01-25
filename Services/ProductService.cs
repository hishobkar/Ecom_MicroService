using Example01.Models;
using Example01.Repositories;

namespace Example01.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;

    public ProductService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product?> GetProductByNameAsync(string productName)
    {
        return await _productRepository.GetProductByNameAsync(productName);
    }

    public async Task AddProductAsync(Product product)
    {
        try
        {
            await _productRepository.AddProductAsync(product);
        } catch(Exception ex)
        {
            throw ex;
        }
    }

    public async Task<bool> UpdateProductAsync(string id, Product updatedProduct)
    {
        try
        {
            return await _productRepository.UpdateProductAsync(id, updatedProduct);
        } catch(Exception ex)
        {
            throw ex;
        }
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        return await _productRepository.DeleteProductAsync(id);
    }

    public async Task<List<Product>> SearchProductsAsync(string keyword)
    {
        return await _productRepository.SearchProductsAsync(keyword);
    }

    public async Task<List<Product>> GetAllProductsAsync(int pageNumber, int pageSize)
    {
        return await _productRepository.GetAllProductsAsync(pageNumber, pageSize);
    }

}
