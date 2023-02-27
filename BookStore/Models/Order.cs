namespace BookStore.Models;

public class Order
{
    public int Id { get; set; }
    public int Qty { get; set; }
<<<<<<< HEAD
    public double Price { get; set; }
=======
    public decimal Price { get; set; }
>>>>>>> c23ec6aae284e20ca4760e07824403b1203ab1c9
    public DateTime OrderTime { get; set; }
    public int BookId { get; set; }
    public int CustomerId { get; set; }
    public virtual Book? Book { get; set; }
    public virtual AppUser? User { get; set; }
}
