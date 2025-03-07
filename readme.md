# Inventory Management System

This project is a **microservices-based Inventory Management System** built using **.NET Core, Entity Framework Core (EF Core), and MS SQL Server**. The system consists of two microservices:

- **InventoryService**: Manages products and stock.
- **OrderService**: Handles orders and updates product stock accordingly.

Each service has its own separate **MS SQL Server database** (`InventoryDB` and `OrderDB`) and communicates using **REST APIs**.

## **Project Structure**

```
InventoryManagementSystem/
├── InventoryService/  # Handles product management
│   ├── Controllers/
│   ├── Data/
│   ├── Models/
│   ├── Repositories/
│   ├── Program.cs
│   ├── appsettings.json
├── OrderService/  # Handles order processing
│   ├── Controllers/
│   ├── Data/
│   ├── Models/
│   ├── Repositories/
│   ├── Program.cs
│   ├── appsettings.json
└── README.md
```

## **1. InventoryService (Product Management)**

### **Model**

```csharp
public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
```

### **DbContext**

```csharp
public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
}
```

### **Product Repository**

```csharp
public class ProductRepository
{
    private readonly InventoryDbContext _context;

    public ProductRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Product> GetProductByIdAsync(int productId)
    {
        return await _context.Products.FindAsync(productId);
    }

    public async Task AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }
}
```

### **Product Controller**

```csharp
[HttpGet("{productId}")]
public async Task<IActionResult> GetProductById(int productId)
{
    var product = await _repository.GetProductByIdAsync(productId);
    if (product == null)
        return NotFound();
    return Ok(product);
}
```

### **Database Configuration & Migration**

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=InventoryDB;Trusted_Connection=True;"
}
```

Run migrations:

```sh
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## **2. OrderService (Order Management)**

### **Model**

```csharp
public class Order
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
```

### **DbContext**

```csharp
public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
    public DbSet<Order> Orders { get; set; }
}
```

### **Order Repository**

```csharp
public class OrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task AddOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }
}
```

### **Order Controller**

```csharp
[HttpPost]
public async Task<IActionResult> PlaceOrder([FromBody] Order order)
{
    var response = await _httpClient.GetAsync($"http://localhost:5118/api/Product/{order.ProductId}");
    if (!response.IsSuccessStatusCode)
        return BadRequest("Product not available.");

    var product = JsonSerializer.Deserialize<Product>(await response.Content.ReadAsStringAsync());
    if (product.Quantity < order.Quantity)
        return BadRequest("Insufficient stock.");

    var updateStock = new StringContent(JsonSerializer.Serialize(product.Quantity - order.Quantity), Encoding.UTF8, "application/json");
    await _httpClient.PutAsync($"http://localhost:5118/api/Product/{order.ProductId}/Stock", updateStock);

    await _orderRepository.AddOrderAsync(order);
    return Ok("Order placed successfully.");
}
```

### **Database Configuration & Migration**

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=OrderDB;Trusted_Connection=True;"
}
```

Run migrations:

```sh
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## **Final Steps**

1. Run `InventoryService` at [**http://localhost:5118/swagger/index.html**](http://localhost:5118/swagger/index.html)
2. Run `OrderService` at [**http://localhost:5058/swagger/index.html**](http://localhost:5058/swagger/index.html)
3. Test the APIs using **Swagger or Postman**

## **Solution Summary**

✅ Two microservices (`InventoryService` & `OrderService`) ✅ Entity Framework Core Code-First approach ✅ Separate SQL Server databases (`InventoryDB` & `OrderDB`) ✅ REST API communication between services ✅ Proper error handling & structured responses

---

🚀 **Happy Coding!** Let me know if you need modifications!
