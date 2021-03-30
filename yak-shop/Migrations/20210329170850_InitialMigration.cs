using Microsoft.EntityFrameworkCore.Migrations;

namespace yak_shop.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YakItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<float>(type: "real", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    ageLastShaved = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YakItems", x => x.Id);
                });

            //migrationBuilder.InsertData(
            //    table: "YakItems",
            //    columns: new[] { "Id", "Age", "Name", "Sex", "ageLastShaved" },
            //    values: new object[] { 1, 3f, "Billy", "f", 3f });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YakItems");
        }
    }
}
