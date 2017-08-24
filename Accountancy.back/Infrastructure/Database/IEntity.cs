namespace Accountancy.Infrastructure.Database
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}