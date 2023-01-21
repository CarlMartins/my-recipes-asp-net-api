using FluentMigrator;

namespace LivroDeReceitas.Infrastructure.Migrations.Versions;

[Migration((long) VersionNumbers.CreateUserTable, "Created user table")]
public class Versao000001 : Migration
{
    public override void Up()
    {
        BaseVersion.InsertDefaultColumns(Create.Table("User"))
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable()
            .WithColumn("Telephone").AsString(15).NotNullable();
    }

    public override void Down()
    { }
}