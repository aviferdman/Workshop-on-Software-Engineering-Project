using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingSystem.Migrations
{
    public partial class MarketMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApartmentNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "dataUsers",
                columns: table => new
                {
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLoggedin = table.Column<bool>(type: "bit", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dataUsers", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "deliveryStatuses",
                columns: table => new
                {
                    PackageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliveryStatuses", x => x.PackageId);
                });

            migrationBuilder.CreateTable(
                name: "marketRulesRequestType1",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    functionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    storeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    discountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketRulesRequestType1", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "marketRulesRequestType2",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    functionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    storeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    policyRuleRelation = table.Column<int>(type: "int", nullable: false),
                    ruleContext = table.Column<int>(type: "int", nullable: false),
                    ruleType = table.Column<int>(type: "int", nullable: false),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    productId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    valueLessThan = table.Column<double>(type: "float", nullable: false),
                    valueGreaterEQThan = table.Column<double>(type: "float", nullable: false),
                    d1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    d2 = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketRulesRequestType2", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "marketRulesRequestType3",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    functionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    storeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketRulesRequestType3", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "marketRulesRequestType4",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    functionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    storeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    discountType = table.Column<int>(type: "int", nullable: false),
                    precent = table.Column<double>(type: "float", nullable: false),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    productId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketRulesRequestType4", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "marketRulesRequestType5",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    functionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    discountRuleRelation = table.Column<int>(type: "int", nullable: false),
                    storeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    discountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    discountId2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    decide = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketRulesRequestType5", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "marketRulesRequestType6",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    functionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    storeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    discountType = table.Column<int>(type: "int", nullable: false),
                    ruleType = table.Column<int>(type: "int", nullable: false),
                    precent = table.Column<double>(type: "float", nullable: false),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    productId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    valueLessThan = table.Column<double>(type: "float", nullable: false),
                    valueGreaterEQThan = table.Column<double>(type: "float", nullable: false),
                    d1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    d2 = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketRulesRequestType6", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "membersShoppingCarts",
                columns: table => new
                {
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_membersShoppingCarts", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "paymentStatuses",
                columns: table => new
                {
                    PaymentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentStatuses", x => x.PaymentId);
                });

            migrationBuilder.CreateTable(
                name: "productHistoryDatas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productHistoryDatas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PurchasePolicy",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasePolicy", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "states",
                columns: table => new
                {
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_states", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "statistics",
                columns: table => new
                {
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    guestsNum = table.Column<int>(type: "int", nullable: false),
                    membersNum = table.Column<int>(type: "int", nullable: false),
                    ownersNum = table.Column<int>(type: "int", nullable: false),
                    managersNum = table.Column<int>(type: "int", nullable: false),
                    adminNum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statistics", x => x.date);
                });

            migrationBuilder.CreateTable(
                name: "purchasedProducts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    ProductHistoryDataid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    storeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _paymentStatusPaymentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    _deliveryStatusPackageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    productHistoriesid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _status = table.Column<bool>(type: "bit", nullable: false)
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
                    sid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _addressid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    purchasePolicyid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    sid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sid1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    appointerID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Owner_sid1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointers", x => new { x.sid, x.username });
                    table.ForeignKey(
                        name: "FK_appointers_states_appointerID",
                        column: x => x.appointerID,
                        principalTable: "states",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appointers_states_username",
                        column: x => x.username,
                        principalTable: "states",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointers_stores_Owner_sid1",
                        column: x => x.Owner_sid1,
                        principalTable: "stores",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appointers_stores_sid1",
                        column: x => x.sid1,
                        principalTable: "stores",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BidsManager",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    sid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidsManager", x => x.id);
                    table.ForeignKey(
                        name: "FK_BidsManager_stores_sid",
                        column: x => x.sid,
                        principalTable: "stores",
                        principalColumn: "sid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "managers",
                columns: table => new
                {
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    sid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    appointerID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_managers", x => new { x.sid, x.username });
                    table.ForeignKey(
                        name: "FK_managers_states_appointerID",
                        column: x => x.appointerID,
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
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    storesid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShoppingCartusername1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ShoppingCartusername = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_membersShoppingBaskets", x => x.id);
                    table.ForeignKey(
                        name: "FK_membersShoppingBaskets_membersShoppingCarts_ShoppingCartusername",
                        column: x => x.ShoppingCartusername,
                        principalTable: "membersShoppingCarts",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_membersShoppingBaskets_membersShoppingCarts_ShoppingCartusername1",
                        column: x => x.ShoppingCartusername1,
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
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _quantity = table.Column<int>(type: "int", nullable: false),
                    _weight = table.Column<double>(type: "float", nullable: false),
                    _price = table.Column<double>(type: "float", nullable: false),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rating = table.Column<int>(type: "int", nullable: false),
                    _storeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    discount = table.Column<double>(type: "float", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Storesid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "BidStates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BidsManagerid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidStates", x => x.id);
                    table.ForeignKey(
                        name: "FK_BidStates_BidsManager_BidsManagerid",
                        column: x => x.BidsManagerid,
                        principalTable: "BidsManager",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productInCarts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    ShoppingBasketid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                        name: "FK_productInCarts_products_productid",
                        column: x => x.productid,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bids_BidStates_stateId",
                        column: x => x.stateId,
                        principalTable: "BidStates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    p = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BidStateid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Managersid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Managerusername = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PurchasePolicyid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prem", x => x.id);
                    table.ForeignKey(
                        name: "FK_Prem_BidStates_BidStateid",
                        column: x => x.BidStateid,
                        principalTable: "BidStates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateIndex(
                name: "IX_appointers_appointerID",
                table: "appointers",
                column: "appointerID");

            migrationBuilder.CreateIndex(
                name: "IX_appointers_Owner_sid1",
                table: "appointers",
                column: "Owner_sid1");

            migrationBuilder.CreateIndex(
                name: "IX_appointers_sid1",
                table: "appointers",
                column: "sid1",
                unique: true,
                filter: "[sid1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_appointers_username",
                table: "appointers",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_stateId",
                table: "Bids",
                column: "stateId",
                unique: true,
                filter: "[stateId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BidsManager_sid",
                table: "BidsManager",
                column: "sid",
                unique: true,
                filter: "[sid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BidStates_BidsManagerid",
                table: "BidStates",
                column: "BidsManagerid");

            migrationBuilder.CreateIndex(
                name: "IX_managers_appointerID",
                table: "managers",
                column: "appointerID");

            migrationBuilder.CreateIndex(
                name: "IX_managers_username",
                table: "managers",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "IX_membersShoppingBaskets_ShoppingCartusername",
                table: "membersShoppingBaskets",
                column: "ShoppingCartusername");

            migrationBuilder.CreateIndex(
                name: "IX_membersShoppingBaskets_ShoppingCartusername1",
                table: "membersShoppingBaskets",
                column: "ShoppingCartusername1");

            migrationBuilder.CreateIndex(
                name: "IX_membersShoppingBaskets_storesid",
                table: "membersShoppingBaskets",
                column: "storesid");

            migrationBuilder.CreateIndex(
                name: "IX_Prem_BidStateid",
                table: "Prem",
                column: "BidStateid");

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
                name: "IX_products_CategoryName",
                table: "products",
                column: "CategoryName");

            migrationBuilder.CreateIndex(
                name: "IX_products_Storesid",
                table: "products",
                column: "Storesid");

            migrationBuilder.CreateIndex(
                name: "IX_purchasedProducts_ProductHistoryDataid",
                table: "purchasedProducts",
                column: "ProductHistoryDataid");

            migrationBuilder.CreateIndex(
                name: "IX_stores__addressid",
                table: "stores",
                column: "_addressid");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointers");

            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "dataUsers");

            migrationBuilder.DropTable(
                name: "marketRulesRequestType1");

            migrationBuilder.DropTable(
                name: "marketRulesRequestType2");

            migrationBuilder.DropTable(
                name: "marketRulesRequestType3");

            migrationBuilder.DropTable(
                name: "marketRulesRequestType4");

            migrationBuilder.DropTable(
                name: "marketRulesRequestType5");

            migrationBuilder.DropTable(
                name: "marketRulesRequestType6");

            migrationBuilder.DropTable(
                name: "Prem");

            migrationBuilder.DropTable(
                name: "productInCarts");

            migrationBuilder.DropTable(
                name: "purchasedProducts");

            migrationBuilder.DropTable(
                name: "statistics");

            migrationBuilder.DropTable(
                name: "transactionStatuses");

            migrationBuilder.DropTable(
                name: "BidStates");

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
                name: "BidsManager");

            migrationBuilder.DropTable(
                name: "states");

            migrationBuilder.DropTable(
                name: "membersShoppingCarts");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "stores");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "PurchasePolicy");
        }
    }
}
