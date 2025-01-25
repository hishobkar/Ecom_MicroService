using Example01.Data;
using Example01.Models;
using MongoDB.Driver;

namespace Example01.Repositories;

public class ProductRepository
{
    private readonly IMongoCollection<Product> _products;

    public ProductRepository(MongoDbContext context)
    {
        _products = context.GetCollection<Product>("Products");
    }

    public async Task<Product?> GetProductByNameAsync(string product_name)
    {
        return await _products.Find(u => u.Name == product_name).FirstOrDefaultAsync();
    }

    public async Task AddProductAsync(Product product)
    {
        try
        {
            await _products.InsertOneAsync(product);
        } catch(Exception e)
        {
            throw e;
        }
    }

    public async Task<bool> UpdateProductAsync(string id, Product updatedProduct)
    {
        var result = await _products.ReplaceOneAsync(p => p.Id == id, updatedProduct);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        var result = await _products.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<List<Product>> SearchProductsAsync(string keyword)
    {
        var filter = Builders<Product>.Filter.Or(
            Builders<Product>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(keyword, "i")),
            Builders<Product>.Filter.Regex(p => p.Description, new MongoDB.Bson.BsonRegularExpression(keyword, "i"))
        );
        return await _products.Find(filter).ToListAsync();
    }
    
    public async Task<List<Product>> GetAllProductsAsync(int pageNumber, int pageSize)
    {
        var products = await _products
            .Find(_ => true)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
        return products;
    }

}