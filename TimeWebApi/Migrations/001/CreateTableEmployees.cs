namespace TimeWebApi.Migrations._001;

using FluentMigrator;
using FluentMigrator.Postgres;

[Migration(202419122056001)]
public sealed class CreateTableEmployees : Migration
{
    public override void Up()
    {
        Create.Table("Employees")
            .WithColumn("Id")
                .AsInt64()
                .PrimaryKey()
                .Identity(PostgresGenerationType.ByDefault)
            .WithColumn("Email")
                .AsString(255)
            .WithColumn("FirstName")
                .AsString(255)
            .WithColumn("LastName")
                .AsString(255)
            .WithColumn("IsDeleted")
                .AsBoolean()
                .WithDefaultValue(false);

        Create.Index("EmployeesEmailIdx")
            .OnTable("Employees")
                .OnColumn("Email")
            .Unique()
            .WithOptions()
                .Filter("\"IsDeleted\" = false");
    }

    public override void Down()
    {
        Delete.Table("Employees");
    }
}
