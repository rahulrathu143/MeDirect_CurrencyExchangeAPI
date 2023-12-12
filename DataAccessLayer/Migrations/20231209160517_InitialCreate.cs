using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cutomerCurrencyEntities",
                columns: table => new
                {
                    CustomerTransactionID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fromCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    toCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cutomerCurrencyEntities", x => x.CustomerTransactionID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cutomerCurrencyEntities");
        }
    }
}
