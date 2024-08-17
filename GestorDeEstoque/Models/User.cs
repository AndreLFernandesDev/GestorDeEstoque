namespace GestorDeEstoque.Models
{
  public class Users
  {
    public int Id { get; private set; }
    public string Name { get; set; }
    public int Age { get; set; }

    public Users(int id, string name, int age)
    {
      Id = id;
      Name = name;
      Age = age;
    }
  }
}