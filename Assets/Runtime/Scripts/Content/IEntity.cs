namespace App.Content
{
    public interface IEntity
    {
        T Get<T>() where T : class;
    }
}