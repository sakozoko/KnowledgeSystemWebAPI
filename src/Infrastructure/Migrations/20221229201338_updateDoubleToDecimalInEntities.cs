using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDoubleToDecimalInEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AlterColumn<decimal>(
                name: "MaxMark",
                table: "Tests",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
            

            migrationBuilder.AlterColumn<decimal>(
                name: "Mark",
                table: "Questions",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AlterColumn<double>(
                name: "MaxMark",
                table: "Tests",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Mark",
                table: "Questions",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);
            
        }
    }
}
