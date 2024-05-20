using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetroCassetteVHS.Infrastructure.Migrations
{
    public partial class AddRentalPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentalPrice",
                table: "Cassettes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentalPrice",
                table: "Cassettes");
        }
    }
}
