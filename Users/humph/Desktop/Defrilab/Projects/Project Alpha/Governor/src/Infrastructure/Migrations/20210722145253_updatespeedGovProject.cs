using Microsoft.EntityFrameworkCore.Migrations;

namespace _.Infrastructure.Migrations
{
    public partial class updatespeedGovProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Products_SpeedGovId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_OwnerId1",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OwnerId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OwnerId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "SpeedGovenors");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SpeedGovenors",
                newName: "PlateNummber");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "SpeedGovenors",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "SpeedGovenors",
                newName: "CartypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_BrandId",
                table: "SpeedGovenors",
                newName: "IX_SpeedGovenors_CartypeId");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "SpeedGovenors",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpeedGovenors",
                table: "SpeedGovenors",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SpeedGovenors_OwnerId",
                table: "SpeedGovenors",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_SpeedGovenors_SpeedGovId",
                table: "Locations",
                column: "SpeedGovId",
                principalTable: "SpeedGovenors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpeedGovenors_Brands_CartypeId",
                table: "SpeedGovenors",
                column: "CartypeId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpeedGovenors_Users_OwnerId",
                table: "SpeedGovenors",
                column: "OwnerId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_SpeedGovenors_SpeedGovId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_SpeedGovenors_Brands_CartypeId",
                table: "SpeedGovenors");

            migrationBuilder.DropForeignKey(
                name: "FK_SpeedGovenors_Users_OwnerId",
                table: "SpeedGovenors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpeedGovenors",
                table: "SpeedGovenors");

            migrationBuilder.DropIndex(
                name: "IX_SpeedGovenors_OwnerId",
                table: "SpeedGovenors");

            migrationBuilder.RenameTable(
                name: "SpeedGovenors",
                newName: "Products");

            migrationBuilder.RenameColumn(
                name: "PlateNummber",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "CartypeId",
                table: "Products",
                newName: "BrandId");

            migrationBuilder.RenameIndex(
                name: "IX_SpeedGovenors_CartypeId",
                table: "Products",
                newName: "IX_Products_BrandId");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId1",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OwnerId1",
                table: "Products",
                column: "OwnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Products_SpeedGovId",
                table: "Locations",
                column: "SpeedGovId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_OwnerId1",
                table: "Products",
                column: "OwnerId1",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
