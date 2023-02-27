namespace BookStore.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Book>? Book { get; set; }
}
