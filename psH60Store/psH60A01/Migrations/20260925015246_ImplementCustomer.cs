using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace psH60A01.Migrations
{
    /// <inheritdoc />
    public partial class ImplementCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CreditCard = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdCat = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFulfilled = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Taxes = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdCatId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true),
                    Manufacturer = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    BuyPrice = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true),
                    SellPrice = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Product_ProductCategory",
                        column: x => x.ProdCatId,
                        principalTable: "ProductCategory",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    ShoppingCartCartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItems_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_ShoppingCarts_CartId",
                        column: x => x.CartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_ShoppingCarts_ShoppingCartCartId",
                        column: x => x.ShoppingCartCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "CartId");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CreditCard", "Email", "FirstName", "LastName", "PhoneNumber", "Province" },
                values: new object[,]
                {
                    { 1, "1234567890123456", "paula@example.com", "Paula", "Selskiy", "8195551234", "QC" },
                    { 2, "2345678901234567", "stella@example.com", "Stella", "Hatton", "8195555678", "ON" },
                    { 3, "3456789012345678", "chaicat@example.com", "Chai", "Hatton", "8195559012", "QC" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "CategoryID", "ProdCat" },
                values: new object[,]
                {
                    { 1, "Crochet hooks" },
                    { 2, "Knitting needles" },
                    { 3, "Yarn" },
                    { 4, "Patterns" },
                    { 5, "Felting tools" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "DateCreated", "DateFulfilled", "Taxes", "Total" },
                values: new object[] { 1, 1, new DateTime(2026, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1.60m, 15.97m });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductID", "BuyPrice", "Description", "Manufacturer", "ProdCatId", "SellPrice", "Stock" },
                values: new object[,]
                {
                    { 1, 4.50m, "Wooden hook", "Red Heart", 1, 5.99m, 25 },
                    { 2, 2.50m, "Plastic hook", "Red Heart", 1, 3.99m, 40 },
                    { 3, 3.60m, "Metal hook", "Lion Brand", 1, 4.99m, 36 },
                    { 4, 6.50m, "Ergonomic hook", "Clover Armour", 1, 10.99m, 10 },
                    { 5, 4.50m, "Wooden needles", "Red Heart", 2, 5.99m, 25 },
                    { 6, 4.50m, "Plastic needles", "Red Heart", 2, 5.99m, 25 },
                    { 7, 4.50m, "Metal needles", "Red Heart", 2, 5.99m, 25 },
                    { 8, 4.50m, "Circular needles", "Red Heart", 2, 5.99m, 25 },
                    { 9, 4.50m, "Merino wool", "Red Heart", 3, 5.99m, 25 },
                    { 10, 4.50m, "Cotton yarn", "Red Heart", 3, 5.99m, 25 },
                    { 11, 4.50m, "Alpacca wool", "Red Heart", 3, 5.99m, 25 },
                    { 12, 4.50m, "Silk yarn", "Red Heart", 3, 5.99m, 25 },
                    { 13, 4.50m, "Knit socks", "Red Heart", 4, 5.99m, 25 },
                    { 14, 4.50m, "Cat amigurumi", "Red Heart", 4, 5.99m, 25 },
                    { 15, 4.50m, "Crochet blanket", "Red Heart", 4, 5.99m, 25 },
                    { 16, 4.50m, "Cozy sweather", "Red Heart", 4, 5.99m, 25 },
                    { 17, 4.50m, "Felting needle", "Red Heart", 5, 5.99m, 25 },
                    { 18, 4.50m, "Felting mat", "Red Heart", 5, 5.99m, 25 },
                    { 19, 4.50m, "Needle handle", "Red Heart", 5, 5.99m, 25 },
                    { 20, 4.50m, "Finger guards", "Red Heart", 5, 5.99m, 25 }
                });

            migrationBuilder.InsertData(
                table: "ShoppingCarts",
                columns: new[] { "CartId", "CustomerId", "DateCreated" },
                values: new object[] { 1, 2, new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "CartItemId", "CartId", "Price", "ProductId", "Quantity", "ShoppingCartCartId" },
                values: new object[,]
                {
                    { 1, 1, 10.99m, 4, 2, null },
                    { 2, 1, 5.99m, 5, 1, null }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "OrderId", "Price", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 5.99m, 1, 1 },
                    { 2, 1, 3.99m, 2, 1 },
                    { 3, 1, 4.99m, 3, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ShoppingCartCartId",
                table: "CartItems",
                column: "ShoppingCartCartId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProdCatId",
                table: "Product",
                column: "ProdCatId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_CustomerId",
                table: "ShoppingCarts",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "ProductCategory");
        }
    }
}
