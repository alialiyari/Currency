using System.Collections.Generic;

namespace Models
{

    public class ServiceDto<T1, T2>
    {
        public T1 D1 { get; set; }
        public T2 D2 { get; set; }
        public byte Status { get; set; } = 1;
        public string Message { get; set; }

    }
    public class ServiceDto<T>
    {
        public T Data { get; set; }
        public byte Status { get; set; } = 1;
        public string Message { get; set; }
    }

    public class ServiceDto
    {
        public byte Status { get; set; } = 1;
        public string Data { get; set; }
        public string Message { get; set; }
    }
    public class ColDataInfo
    {
        public string Name { get; set; }
        public int? Sum { get; set; }
        public int? Average { get; set; }
    }

    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public List<ColDataInfo> ColDataInfos { get; set; }

        //public static implicit operator PagedResultDto<T>(List<global::Ticket.ListRelatedTask> v)
        //{
        //    throw new NotImplementedException();
        //}

        //public static implicit operator PagedResultDto<T>(List<global::Ticket.ListRelatedTask> v)
        //{
        //    throw new NotImplementedException();
        //}
    }

    public class FileDto
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string Header { get; set; }
        public long Size { get; set; }
        public StatusEnum Status { get; set; }

        public enum StatusEnum
        {
            Added,
            Deleted,
            Unchanged
        }
    }
}
