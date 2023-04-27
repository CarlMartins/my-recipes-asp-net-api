using FluentMigrator;

namespace RecipeBook.Infrastructure.Migrations.Versions;

[Migration((long) VersionNumbers.CreateUserTable, "Created user table")]
public class Versao000001 : Migration
{
    public override void Up()
    {
        BaseVersion.InsertDefaultColumns(Create.Table("Users"))
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable()
            .WithColumn("Contact").AsString(15).NotNullable();
    }

    public override void Down()
    { }
}