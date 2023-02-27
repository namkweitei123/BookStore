namespace BookStore.Models;

public class Order
{
    public int Id { get; set; }
    public int Qty { get; set; }
    public decimal Price { get; set; }
    public DateTime OrderTime { get; set; }
    public int BookId { get; set; }
    public int CustomerId { get; set; }
    public virtual Book? Book { get; set; }
    public virtual AppUser? User { get; set; }
}
