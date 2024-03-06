namespace gurukul.interfaces;

public interface IModelDao<T>
{
    public List<T> GetList();

    public T GetById(string id);

    public void Create(T data);

    public void DeleteById(string id);

    public void UpdateById(string id, T data);
}