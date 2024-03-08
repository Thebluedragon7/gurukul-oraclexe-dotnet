namespace gurukul.interfaces;

public enum SortByEnum
{
    ASCENDING,
    DESCENDING
}

public interface IModelDao<T>
{
    public List<T> GetList();

    public List<T> GetList(string col, SortByEnum sortBy = SortByEnum.ASCENDING);

    public List<T> GetList(string searchQuery, string searchCol);

    public List<T> GetList(string searchQuery, string searchCol, string col, SortByEnum sortBy = SortByEnum.ASCENDING);

    public T GetById(string id);

    public void Create(T data);

    public void DeleteById(string id);

    public void UpdateById(string id, T data);
}