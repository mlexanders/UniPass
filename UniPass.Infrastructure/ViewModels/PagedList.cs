namespace UniPass.Infrastructure.ViewModels;

public class PagedList<T>
{
    public PagedList()
    {
    }
    public PagedList(int page, int pageSize, int count, List<T> items)
    {
        Page = page;
        PageSize = pageSize;
        Count = count;
        Items = items;
    }
    
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public List<T> Items { get; set; }
}