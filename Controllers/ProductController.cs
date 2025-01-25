using Example01.Models;
using Example01.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Example01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{name}")]
    [Authorize]
    public async Task<IActionResult> GetProductByName(string name)
    {
        var product = await _productService.GetProductByNameAsync(name);
        if (product == null)
            return NotFound();
        return Ok(product);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
        try
        {
            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductByName), new { name = product.Name }, product);
        } catch(Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateProduct(string id, [FromBody] Product updatedProduct)
    {
        try
        {
            var result = await _productService.UpdateProductAsync(id, updatedProduct);
            if (!result)
                return NotFound();
            return NoContent();
        } catch(Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        var result = await _productService.DeleteProductAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> SearchProducts([FromQuery] string keyword)
    {
        var products = await _productService.SearchProductsAsync(keyword);
        return Ok(products);
    }
    
    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var products = await _productService.GetAllProductsAsync(pageNumber, pageSize);
        return Ok(products);
    }

}
