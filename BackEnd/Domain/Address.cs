namespace Domain
{
public class Address : IEntity
{
   public int Id { get; set; }
   public string Country { get; set; }
   public string State { get; set; }
   public string City { get; set; }
   public string Street { get; set; }
   public string Number { get; set; }
}
}