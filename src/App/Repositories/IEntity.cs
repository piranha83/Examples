namespace App.Repositories
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}