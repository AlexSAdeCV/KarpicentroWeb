using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarpicentroWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Direction",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Neighborhood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Delegations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExtNum = table.Column<int>(type: "int", nullable: false),
                    IntNum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Direction", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<int>(type: "int", nullable: false),
                    NameSupplier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameContact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idDirections = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Supplier_Direction_idDirections",
                        column: x => x.idDirections,
                        principalTable: "Direction",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UseDir",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUser = table.Column<int>(type: "int", nullable: false),
                    idDirections = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UseDir", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UseDir_Direction_idDirections",
                        column: x => x.idDirections,
                        principalTable: "Direction",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UseDir_User_idUser",
                        column: x => x.idUser,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductFeatures = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Depth = table.Column<int>(type: "int", nullable: false),
                    Categories = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Featured = table.Column<int>(type: "int", nullable: false),
                    idSupplier = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Product_Supplier_idSupplier",
                        column: x => x.idSupplier,
                        principalTable: "Supplier",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SwitchCartBuy = table.Column<int>(type: "int", nullable: false),
                    IDOrder = table.Column<int>(type: "int", nullable: false),
                    idProduct = table.Column<int>(type: "int", nullable: false),
                    idUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cart_Product_idProduct",
                        column: x => x.idProduct,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cart_User_idUser",
                        column: x => x.idUser,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterProd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idProducts = table.Column<int>(type: "int", nullable: false),
                    idColors = table.Column<int>(type: "int", nullable: false),
                    idMaterials = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterProd", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InterProd_Colors_idColors",
                        column: x => x.idColors,
                        principalTable: "Colors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterProd_Materials_idMaterials",
                        column: x => x.idMaterials,
                        principalTable: "Materials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterProd_Product_idProducts",
                        column: x => x.idProducts,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_idProduct",
                table: "Cart",
                column: "idProduct");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_idUser",
                table: "Cart",
                column: "idUser");

            migrationBuilder.CreateIndex(
                name: "IX_InterProd_idColors",
                table: "InterProd",
                column: "idColors");

            migrationBuilder.CreateIndex(
                name: "IX_InterProd_idMaterials",
                table: "InterProd",
                column: "idMaterials");

            migrationBuilder.CreateIndex(
                name: "IX_InterProd_idProducts",
                table: "InterProd",
                column: "idProducts");

            migrationBuilder.CreateIndex(
                name: "IX_Product_idSupplier",
                table: "Product",
                column: "idSupplier");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_idDirections",
                table: "Supplier",
                column: "idDirections");

            migrationBuilder.CreateIndex(
                name: "IX_UseDir_idDirections",
                table: "UseDir",
                column: "idDirections");

            migrationBuilder.CreateIndex(
                name: "IX_UseDir_idUser",
                table: "UseDir",
                column: "idUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "InterProd");

            migrationBuilder.DropTable(
                name: "UseDir");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "Direction");
        }
    }
}
