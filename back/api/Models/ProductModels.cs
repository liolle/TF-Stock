namespace stock.api.models;

public class AddProductModel {
    public required string Name { get; set; }

    public int Quantity { get; set; } 

    public required string GTIN { get; set; } 

    public int Price { get; set; } 
}

public class UpdateProductModel {
    public  int Id { get; set; }

    public int Quantity { get; set; } 
}

