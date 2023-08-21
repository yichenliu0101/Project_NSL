namespace Nsl_Core.Models.Infra
{
    public static class Pagination
    {
        public static List<T> PageList<T>(this List<T> list, int pageSize, int pageIndex)
        {
            return list.Skip((pageIndex-1) * pageSize).Take(pageSize).ToList();
        }
    }
}
