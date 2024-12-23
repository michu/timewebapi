namespace TimeWebApi.Migrations._001;

using FluentMigrator;
using FluentMigrator.Postgres;
using System.Data;

[Migration(202419122056002)]
public sealed class CreateTableTimeEntries : Migration
{
    public override void Up()
    {
        Create.Table("TimeEntries")
            .WithColumn("Id")
                .AsInt64()
                .PrimaryKey()
                .Identity(PostgresGenerationType.ByDefault)
            .WithColumn("EmployeeId")
                .AsInt64()
            .WithColumn("Date")
                .AsDate()
            .WithColumn("HoursWorked")
                .AsDecimal(9, 2)
        ;

        Create.ForeignKey()
            .FromTable("TimeEntries")
                .ForeignColumn("EmployeeId")
            .ToTable("Employees")
                .PrimaryColumn("Id")
            .OnDelete(Rule.Cascade)
            .OnUpdate(Rule.Cascade)
        ;

        Create.Index("TimeEntriesEmployeeIdDateIdx")
            .OnTable("TimeEntries")
                .OnColumn("EmployeeId")
                    .Unique()
                .OnColumn("Date")
                    .Unique();
        ;

        Create.Index("TimeEntriesEmployeeIdIdx")
            .OnTable("TimeEntries")
                .OnColumn("EmployeeId")
        ;
    }

    public override void Down()
    {
        Delete.Table("TimeEntries");
    }
}
