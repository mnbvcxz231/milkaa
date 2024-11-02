using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var productRepository = new ProductRepository();

app.MapGet("/", () => productRepository.GetAll());

app.MapGet("/products/{id}", (int id) => {
    var product = productRepository.Get(id);
    return Results.Ok(product);
    });

app.MapPost("/products", (Product NewProduct) => 
{
    var add_Product = productRepository.Add(NewProduct);
    return Results.Created($"/products/{add_Product.Id}", add_Product);
});

app.MapPut("/products/{id}", (int id, Product putProduct) =>
{
    putProduct.Id = id;
    var updated = productRepository.Update(putProduct);
    return Results.Ok(putProduct);
});
 
app.MapDelete("/products/delete/{id}", (int id) => 
{ 
    productRepository.Remove(id); 
    return Results.NoContent(); 
});

app.Run();

public class ProductRepository{
    private List<Product> products = new List<Product>();
    private int _nextId = 1;

    public ProductRepository(){
        Add(new Product {Name = "Кефир", Category = "Молочка", Price = 130});
        Add(new Product {Name = "Ряженка", Category = "Молочка", Price = 100});
        Add(new Product {Name = "Катык", Category = "Молочка", Price = 130});
    }
    public IEnumerable<Product>GetAll(){
        return products;
    }
    public Product Get (int id){
        return products.Find(x => x.Id == id);
    }
    public Product Add(Product item){
        item.Id = _nextId++;
        products.Add(item);
        return item;
    }
    public void Remove(int id){
        products.RemoveAll(x => x.Id == id);
    }

    public bool Update(Product item){
        int index = products.FindIndex(x => x.Id == item.Id);
        if (index > -1){
            return false;
        }
        products.RemoveAt(index);
        products.Add(item);
        return true;
    }
}
 public class Product{
    public int Id{get;set;}
    public string Name{get;set;}
    public string Category{get;set;}
    public int Price{get;set;}
 }

