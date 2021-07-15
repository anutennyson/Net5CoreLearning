namespace Catalog.Dtos{
 public record ItemDto
    {
        public Guid Id {get; init;} // in init... value can be set only when the object is created
    
    public string Name {get; init;}
    public decimal Price {get; init;}
    public DateTimeOffset CreatedDate {get; init;}
    
    }
}