namespace TimeWebApi.TypeHandlers;

using Dapper;
using System.Data;

public sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly date)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = date.ToDateTime(new TimeOnly(0, 0, 0));
    }

    public override DateOnly Parse(object value)
    {
        return DateOnly.FromDateTime((DateTime)value);
    }
}
