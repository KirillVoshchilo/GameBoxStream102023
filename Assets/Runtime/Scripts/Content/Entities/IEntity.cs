namespace App.Content.Entities
{
    public interface IEntity
    {
        T Get<T>() where T : class;
    }
}