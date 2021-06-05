﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingSystem.Migrations
{
    public partial class firstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    _state = table.Column<string>(type: "TEXT", nullable: true),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    Street = table.Column<string>(type: "TEXT", nullable: true),
                    ApartmentNum = table.Column<string>(type: "TEXT", nullable: true),
                    Zip = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    nameId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "dataUsers",
                columns: table => new
                {
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: true),
                    IsLoggedin = table.Column<bool>(type: "INTEGER", nullable: false),
                    phone = table.Column<string>(type: "TEXT", nullable: true),
                    isAdmin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dataUsers", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "deliveryStatuses",
                columns: table => new
                {
                    PackageId = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    StoreId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    StoreName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliveryStatuses", x => x.PackageId);
                });

            migrationBuilder.CreateTable(
                name: "membersShoppingCarts",
                columns: table => new
                {
                    username = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_membersShoppingCarts", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "paymentStatuses",
                columns: table => new
                {
                    PaymentId = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    StoreId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentStatuses", x => x.PaymentId);
                });

            migrationBuilder.CreateTable(
                name: "productHistoryDatas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productHistoryDatas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PurchasePolicy",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasePolicy", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "states",
                columns: table => new
                {
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_states", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "purchasedProducts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    price = table.Column<double>(type: "REAL", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: true),
                    quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductHistoryDataid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchasedProducts", x => x.id);
                    table.ForeignKey(
                        name: "FK_purchasedProducts_productHistoryDatas_ProductHistoryDataid",
                        column: x => x.ProductHistoryDataid,
                        principalTable: "productHistoryDatas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transactionStatuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    username = table.Column<string>(type: "TEXT", nullable: true),
                    storeID = table.Column<Guid>(type: "TEXT", nullable: false),
                    _paymentStatusPaymentId = table.Column<string>(type: "TEXT", nullable: true),
                    _deliveryStatusPackageId = table.Column<string>(type: "TEXT", nullable: true),
                    productHistoriesid = table.Column<Guid>(type: "TEXT", nullable: true),
                    _status = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactionStatuses", x => x.id);
                    table.ForeignKey(
                        name: "FK_transactionStatuses_deliveryStatuses__deliveryStatusPackageId",
                        column: x => x._deliveryStatusPackageId,
                        principalTable: "deliveryStatuses",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactionStatuses_paymentStatuses__paymentStatusPaymentId",
                        column: x => x._paymentStatusPaymentId,
                        principalTable: "paymentStatuses",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactionStatuses_productHistoryDatas_productHistoriesid",
                        column: x => x.productHistoriesid,
                        principalTable: "productHistoryDatas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stores",
                columns: table => new
                {
                    sid = table.Column<Guid>(type: "TEXT", nullable: false),
                    foundersid = table.Column<Guid>(type: "TEXT", nullable: true),
                    founderusername = table.Column<string>(type: "TEXT", nullable: true),
                    name = table.Column<string>(type: "TEXT", nullable: true),
                    _addressid = table.Column<Guid>(type: "TEXT", nullable: true),
                    purchasePolicyid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stores", x => x.sid);
                    table.ForeignKey(
                        name: "FK_stores_addresses__addressid",
                        column: x => x._addressid,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stores_PurchasePolicy_purchasePolicyid",
                        column: x => x.purchasePolicyid,
                        principalTable: "PurchasePolicy",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "appointers",
                columns: table => new
                {
                    sid = table.Column<Guid>(type: "TEXT", nullable: false),
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    musername = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    sid1 = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointers", x => new { x.sid, x.username });
                    table.ForeignKey(
                        name: "FK_appointers_states_musername",
                        column: x => x.musername,
                        principalTable: "states",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointers_states_username",
                        column: x => x.username,
                        principalTable: "states",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointers_stores_sid",
                        column: x => x.sid,
                        principalTable: "stores",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointers_stores_sid1",
                        column: x => x.sid1,
                        principalTable: "stores",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "managers",
                columns: table => new
                {
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    sid = table.Column<Guid>(type: "TEXT", nullable: false),
                    musername = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_managers", x => new { x.sid, x.username });
                    table.ForeignKey(
                        name: "FK_managers_states_musername",
                        column: x => x.musername,
                        principalTable: "states",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_managers_states_username",
                        column: x => x.username,
                        principalTable: "states",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_managers_stores_sid",
                        column: x => x.sid,
                        principalTable: "stores",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "membersShoppingBaskets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    shoppingCartusername = table.Column<string>(type: "TEXT", nullable: true),
                    storesid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_membersShoppingBaskets", x => x.id);
                    table.ForeignKey(
                        name: "FK_membersShoppingBaskets_membersShoppingCarts_shoppingCartusername",
                        column: x => x.shoppingCartusername,
                        principalTable: "membersShoppingCarts",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_membersShoppingBaskets_stores_storesid",
                        column: x => x.storesid,
                        principalTable: "stores",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    _name = table.Column<string>(type: "TEXT", nullable: true),
                    _quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    _weight = table.Column<double>(type: "REAL", nullable: false),
                    _price = table.Column<double>(type: "REAL", nullable: false),
                    category = table.Column<string>(type: "TEXT", nullable: true),
                    rating = table.Column<int>(type: "INTEGER", nullable: false),
                    _storeName = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: true),
                    Storesid = table.Column<Guid>(type: "TEXT", nullable: true),
                    Storesid1 = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_products_categories_CategoryName",
                        column: x => x.CategoryName,
                        principalTable: "categories",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_products_stores_Storesid",
                        column: x => x.Storesid,
                        principalTable: "stores",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_products_stores_Storesid1",
                        column: x => x.Storesid1,
                        principalTable: "stores",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bid",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    StoreId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Appointersid = table.Column<Guid>(type: "TEXT", nullable: true),
                    Appointerusername = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bid_appointers_Appointersid_Appointerusername",
                        columns: x => new { x.Appointersid, x.Appointerusername },
                        principalTable: "appointers",
                        principalColumns: new[] { "sid", "username" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    p = table.Column<string>(type: "TEXT", nullable: true),
                    Managersid = table.Column<Guid>(type: "TEXT", nullable: true),
                    Managerusername = table.Column<string>(type: "TEXT", nullable: true),
                    PurchasePolicyid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prem", x => x.id);
                    table.ForeignKey(
                        name: "FK_Prem_managers_Managersid_Managerusername",
                        columns: x => new { x.Managersid, x.Managerusername },
                        principalTable: "managers",
                        principalColumns: new[] { "sid", "username" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prem_PurchasePolicy_PurchasePolicyid",
                        column: x => x.PurchasePolicyid,
                        principalTable: "PurchasePolicy",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "productInCarts",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    productid = table.Column<Guid>(type: "TEXT", nullable: true),
                    quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ShoppingBasketid = table.Column<Guid>(type: "TEXT", nullable: true),
                    ShoppingBasketid1 = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productInCarts", x => x.id);
                    table.ForeignKey(
                        name: "FK_productInCarts_membersShoppingBaskets_ShoppingBasketid",
                        column: x => x.ShoppingBasketid,
                        principalTable: "membersShoppingBaskets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_productInCarts_membersShoppingBaskets_ShoppingBasketid1",
                        column: x => x.ShoppingBasketid1,
                        principalTable: "membersShoppingBaskets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_productInCarts_products_productid",
                        column: x => x.productid,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointers_musername",
                table: "appointers",
                column: "musername");

            migrationBuilder.CreateIndex(
                name: "IX_appointers_sid",
                table: "appointers",
                column: "sid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_appointers_sid1",
                table: "appointers",
                column: "sid1");

            migrationBuilder.CreateIndex(
                name: "IX_appointers_username",
                table: "appointers",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "IX_Bid_Appointersid_Appointerusername",
                table: "Bid",
                columns: new[] { "Appointersid", "Appointerusername" });

            migrationBuilder.CreateIndex(
                name: "IX_managers_musername",
                table: "managers",
                column: "musername");

            migrationBuilder.CreateIndex(
                name: "IX_managers_username",
                table: "managers",
                column: "username");


            migrationBuilder.CreateIndex(
                name: "IX_membersShoppingBaskets_ShoppingCartusername",
                table: "membersShoppingBaskets",
                column: "ShoppingCartusername");

            migrationBuilder.CreateIndex(
                name: "IX_membersShoppingBaskets_storesid",
                table: "membersShoppingBaskets",
                column: "storesid");

            migrationBuilder.CreateIndex(
                name: "IX_Prem_Managersid_Managerusername",
                table: "Prem",
                columns: new[] { "Managersid", "Managerusername" });

            migrationBuilder.CreateIndex(
                name: "IX_Prem_PurchasePolicyid",
                table: "Prem",
                column: "PurchasePolicyid");

            migrationBuilder.CreateIndex(
                name: "IX_productInCarts_productid",
                table: "productInCarts",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_productInCarts_ShoppingBasketid",
                table: "productInCarts",
                column: "ShoppingBasketid");

            migrationBuilder.CreateIndex(
                name: "IX_productInCarts_ShoppingBasketid1",
                table: "productInCarts",
                column: "ShoppingBasketid1");

            migrationBuilder.CreateIndex(
                name: "IX_products_CategoryName",
                table: "products",
                column: "CategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_products_Storesid",
                table: "products",
                column: "Storesid");

            migrationBuilder.CreateIndex(
                name: "IX_products_Storesid1",
                table: "products",
                column: "Storesid1");

            migrationBuilder.CreateIndex(
                name: "IX_purchasedProducts_ProductHistoryDataid",
                table: "purchasedProducts",
                column: "ProductHistoryDataid");

            migrationBuilder.CreateIndex(
                name: "IX_stores__addressid",
                table: "stores",
                column: "_addressid");

            migrationBuilder.CreateIndex(
                name: "IX_stores_foundersid_founderusername",
                table: "stores",
                columns: new[] { "foundersid", "founderusername" });

            migrationBuilder.CreateIndex(
                name: "IX_stores_purchasePolicyid",
                table: "stores",
                column: "purchasePolicyid");

            migrationBuilder.CreateIndex(
                name: "IX_transactionStatuses__deliveryStatusPackageId",
                table: "transactionStatuses",
                column: "_deliveryStatusPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_transactionStatuses__paymentStatusPaymentId",
                table: "transactionStatuses",
                column: "_paymentStatusPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_transactionStatuses_productHistoriesid",
                table: "transactionStatuses",
                column: "productHistoriesid");

            migrationBuilder.AddForeignKey(
                name: "FK_stores_appointers_foundersid_founderusername",
                table: "stores",
                columns: new[] { "foundersid", "founderusername" },
                principalTable: "appointers",
                principalColumns: new[] { "sid", "username" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointers_states_musername",
                table: "appointers");

            migrationBuilder.DropForeignKey(
                name: "FK_appointers_states_username",
                table: "appointers");

            migrationBuilder.DropForeignKey(
                name: "FK_appointers_stores_sid",
                table: "appointers");

            migrationBuilder.DropForeignKey(
                name: "FK_appointers_stores_sid1",
                table: "appointers");

            migrationBuilder.DropTable(
                name: "Bid");

            migrationBuilder.DropTable(
                name: "dataUsers");

            migrationBuilder.DropTable(
                name: "Prem");

            migrationBuilder.DropTable(
                name: "productInCarts");

            migrationBuilder.DropTable(
                name: "purchasedProducts");

            migrationBuilder.DropTable(
                name: "transactionStatuses");

            migrationBuilder.DropTable(
                name: "managers");

            migrationBuilder.DropTable(
                name: "membersShoppingBaskets");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "deliveryStatuses");

            migrationBuilder.DropTable(
                name: "paymentStatuses");

            migrationBuilder.DropTable(
                name: "productHistoryDatas");

            migrationBuilder.DropTable(
                name: "membersShoppingCarts");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "states");

            migrationBuilder.DropTable(
                name: "stores");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "appointers");

            migrationBuilder.DropTable(
                name: "PurchasePolicy");
        }
    }
}