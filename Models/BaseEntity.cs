using SqlSugar;

namespace Test003.Models
{
    public class BaseEntity : IDeleted
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "主键", IsOnlyIgnoreUpdate = true)]
        public Guid ID { get; set; }

        [SugarColumn(ColumnDescription = "是否删除", DefaultValue = "0", IsOnlyIgnoreInsert = true, CreateTableFieldSort = -1)]
        public bool IsDeleted { get; set; }

        [SugarColumn(ColumnDescription = "创建时间", InsertServerTime = true, IsOnlyIgnoreUpdate = true, CreateTableFieldSort = 98)]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnDescription = "修改时间", UpdateServerTime = true, IsOnlyIgnoreUpdate = false, CreateTableFieldSort = 99)]
        public DateTime? UpdateTime { get; set; }
    }

    public interface IDeleted
    {
        [SugarColumn(ColumnDescription = "是否删除", DefaultValue = "0", IsOnlyIgnoreInsert = true, CreateTableFieldSort = -1)]
        public bool IsDeleted { get; set; }
    }
}
