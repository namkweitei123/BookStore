namespace BookStore.Models;

public class Order
{
    public int Id { get; set; }
    public int Qty { get; set; }
    public double Price { get; set; }
    public DateTime OrderTime { get; set; }
    public int BookId { get; set; }
    public int Count { get; set; }
    public virtual Book? Book { get; set; }
    public string AppUserId { get; set; }
    public virtual AppUser? User { get; set; }
}
