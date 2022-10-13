using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.HomeLogix.SqlServer.Migrations
{
    public partial class Startup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvancedFinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Table = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FromFormula = table.Column<string>(type: "ntext", nullable: true),
                    RefreshRate = table.Column<byte>(type: "tinyint", nullable: true),
                    RefreshValue = table.Column<int>(type: "integer", nullable: true),
                    RefreshCondition = table.Column<byte>(type: "tinyint", nullable: true),
                    YellowAlert = table.Column<int>(type: "integer", nullable: true),
                    RedAlert = table.Column<int>(type: "integer", nullable: true),
                    Disabled = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountType = table.Column<byte>(type: "tinyint", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    ProjectedEndingBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    ProjectedLowestBalanceDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProjectedLowestBalanceAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MonthlyBudgetDeposits = table.Column<decimal>(type: "numeric", nullable: false),
                    MonthlyBudgetWithdrawals = table.Column<decimal>(type: "numeric", nullable: false),
                    LastGenerationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Notes = table.Column<string>(type: "ntext", nullable: true),
                    ShowInGraph = table.Column<bool>(type: "bit", nullable: false),
                    LastCompletedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BudgetItemSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsIncome = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetItemSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemMaster",
                columns: table => new
                {
                    HouseholdName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemMaster", x => x.HouseholdName);
                });

            migrationBuilder.CreateTable(
                name: "AdvancedFindColumns",
                columns: table => new
                {
                    AdvancedFindId = table.Column<int>(type: "integer", nullable: false),
                    ColumnId = table.Column<int>(type: "integer", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryTableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryFieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PercentWidth = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Formula = table.Column<string>(type: "ntext", nullable: true),
                    FieldDataType = table.Column<byte>(type: "tinyint", nullable: false),
                    DecimalFormatType = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFindColumns", x => new { x.AdvancedFindId, x.ColumnId });
                    table.ForeignKey(
                        name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                        column: x => x.AdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvancedFindFilters",
                columns: table => new
                {
                    AdvancedFindId = table.Column<int>(type: "integer", nullable: false),
                    FilterId = table.Column<int>(type: "integer", nullable: false),
                    LeftParentheses = table.Column<byte>(type: "tinyint", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryTableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryFieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Operand = table.Column<byte>(type: "tinyint", nullable: false),
                    SearchForValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Formula = table.Column<string>(type: "ntext", nullable: true),
                    FormulaDataType = table.Column<byte>(type: "tinyint", nullable: false),
                    FormulaDisplayValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SearchForAdvancedFindId = table.Column<int>(type: "integer", nullable: true),
                    CustomDate = table.Column<bool>(type: "bit", nullable: false),
                    RightParentheses = table.Column<byte>(type: "tinyint", nullable: false),
                    EndLogic = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFindFilters", x => new { x.AdvancedFindId, x.FilterId });
                    table.ForeignKey(
                        name: "FK_AdvancedFindFilters_AdvancedFinds_AdvancedFindId",
                        column: x => x.AdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvancedFindFilters_AdvancedFinds_SearchForAdvancedFindId",
                        column: x => x.SearchForAdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BankAccountPeriodHistory",
                columns: table => new
                {
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    PeriodType = table.Column<byte>(type: "tinyint", nullable: false),
                    PeriodEndingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TotalDeposits = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalWithdrawals = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountPeriodHistory", x => new { x.BankAccountId, x.PeriodType, x.PeriodEndingDate });
                    table.ForeignKey(
                        name: "FK_BankAccountPeriodHistory_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    RecurringPeriod = table.Column<int>(type: "integer", nullable: false),
                    RecurringType = table.Column<byte>(type: "tinyint", nullable: false),
                    StartingDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndingDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TransferToBankAccountId = table.Column<int>(type: "integer", nullable: true),
                    MonthlyAmount = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    CurrentMonthAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrentMonthEnding = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastCompletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetItems_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetItems_BankAccounts_TransferToBankAccountId",
                        column: x => x.TransferToBankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BankAccountRegisterItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisterGuid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    ItemDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    BudgetItemId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProjectedAmount = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    IsNegative = table.Column<bool>(type: "bit", nullable: false),
                    ActualAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    TransferRegisterGuid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    BankText = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountRegisterItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccountRegisterItems_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccountRegisterItems_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetPeriodHistory",
                columns: table => new
                {
                    BudgetItemId = table.Column<int>(type: "integer", nullable: false),
                    PeriodType = table.Column<byte>(type: "tinyint", nullable: false),
                    PeriodEndingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProjectedAmount = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    ActualAmount = table.Column<decimal>(type: "numeric(38,17)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetPeriodHistory", x => new { x.BudgetItemId, x.PeriodType, x.PeriodEndingDate });
                    table.ForeignKey(
                        name: "FK_BudgetPeriodHistory_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankAccountId = table.Column<int>(type: "integer", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    BudgetItemId = table.Column<int>(type: "integer", nullable: true),
                    TransferToBankAccountId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProjectedAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    ActualAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    BankText = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Id);
                    table.ForeignKey(
                        name: "FK_History_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_History_BankAccounts_TransferToBankAccountId",
                        column: x => x.TransferToBankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_History_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QifMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BudgetId = table.Column<int>(type: "integer", nullable: false),
                    SourceId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QifMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QifMaps_BudgetItems_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QifMaps_BudgetItemSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "BudgetItemSources",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BankAccountRegisterItemAmountDetails",
                columns: table => new
                {
                    RegisterId = table.Column<int>(type: "integer", nullable: false),
                    DetailId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    SourceId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    BankText = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountRegisterItemAmountDetails", x => new { x.RegisterId, x.DetailId });
                    table.ForeignKey(
                        name: "FK_BankAccountRegisterItemAmountDetails_BankAccountRegisterItems_RegisterId",
                        column: x => x.RegisterId,
                        principalTable: "BankAccountRegisterItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccountRegisterItemAmountDetails_BudgetItemSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "BudgetItemSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SourceHistory",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "integer", nullable: false),
                    DetailId = table.Column<int>(type: "integer", nullable: false),
                    SourceId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    BankText = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceHistory", x => new { x.HistoryId, x.DetailId });
                    table.ForeignKey(
                        name: "FK_SourceHistory_BudgetItemSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "BudgetItemSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SourceHistory_History_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "History",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankTransactions",
                columns: table => new
                {
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    TransactionId = table.Column<int>(type: "integer", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    BankTransactionText = table.Column<string>(type: "nvarchar", nullable: true),
                    BudgetId = table.Column<int>(type: "integer", nullable: true),
                    SourceId = table.Column<int>(type: "integer", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    QifMapId = table.Column<int>(type: "integer", nullable: true),
                    MapTransaction = table.Column<bool>(type: "bit", nullable: false),
                    TransactionType = table.Column<byte>(type: "tinyint", nullable: false),
                    BankAccountRegisterItemAmountDetailDetailId = table.Column<int>(type: "integer", nullable: true),
                    BankAccountRegisterItemAmountDetailRegisterId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactions", x => new { x.BankAccountId, x.TransactionId });
                    table.ForeignKey(
                        name: "FK_BankTransactions_BankAccountRegisterItemAmountDetails_BankAccountRegisterItemAmountDetailRegisterId_BankAccountRegisterItemA~",
                        columns: x => new { x.BankAccountRegisterItemAmountDetailRegisterId, x.BankAccountRegisterItemAmountDetailDetailId },
                        principalTable: "BankAccountRegisterItemAmountDetails",
                        principalColumns: new[] { "RegisterId", "DetailId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankTransactions_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BankTransactions_BudgetItems_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BankTransactions_BudgetItemSources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "BudgetItemSources",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BankTransactions_QifMaps_QifMapId",
                        column: x => x.QifMapId,
                        principalTable: "QifMaps",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BankTransactionBudget",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "integer", nullable: false),
                    TransactionId = table.Column<int>(type: "integer", nullable: false),
                    RowId = table.Column<int>(type: "integer", nullable: false),
                    BudgetItemId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactionBudget", x => new { x.BankId, x.TransactionId, x.RowId });
                    table.ForeignKey(
                        name: "FK_BankTransactionBudget_BankTransactions_BankId_TransactionId",
                        columns: x => new { x.BankId, x.TransactionId },
                        principalTable: "BankTransactions",
                        principalColumns: new[] { "BankAccountId", "TransactionId" });
                    table.ForeignKey(
                        name: "FK_BankTransactionBudget_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvancedFindFilters_SearchForAdvancedFindId",
                table: "AdvancedFindFilters",
                column: "SearchForAdvancedFindId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountRegisterItemAmountDetails_SourceId",
                table: "BankAccountRegisterItemAmountDetails",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountRegisterItems_BankAccountId",
                table: "BankAccountRegisterItems",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountRegisterItems_BudgetItemId",
                table: "BankAccountRegisterItems",
                column: "BudgetItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactionBudget_BudgetItemId",
                table: "BankTransactionBudget",
                column: "BudgetItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_BankAccountRegisterItemAmountDetailRegisterId_BankAccountRegisterItemAmountDetailDetailId",
                table: "BankTransactions",
                columns: new[] { "BankAccountRegisterItemAmountDetailRegisterId", "BankAccountRegisterItemAmountDetailDetailId" });

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_BudgetId",
                table: "BankTransactions",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_QifMapId",
                table: "BankTransactions",
                column: "QifMapId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_SourceId",
                table: "BankTransactions",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_BankAccountId",
                table: "BudgetItems",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_TransferToBankAccountId",
                table: "BudgetItems",
                column: "TransferToBankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_History_BankAccountId",
                table: "History",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_History_BudgetItemId",
                table: "History",
                column: "BudgetItemId");

            migrationBuilder.CreateIndex(
                name: "IX_History_TransferToBankAccountId",
                table: "History",
                column: "TransferToBankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_QifMaps_BudgetId",
                table: "QifMaps",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_QifMaps_SourceId",
                table: "QifMaps",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceHistory_SourceId",
                table: "SourceHistory",
                column: "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvancedFindColumns");

            migrationBuilder.DropTable(
                name: "AdvancedFindFilters");

            migrationBuilder.DropTable(
                name: "BankAccountPeriodHistory");

            migrationBuilder.DropTable(
                name: "BankTransactionBudget");

            migrationBuilder.DropTable(
                name: "BudgetPeriodHistory");

            migrationBuilder.DropTable(
                name: "SourceHistory");

            migrationBuilder.DropTable(
                name: "SystemMaster");

            migrationBuilder.DropTable(
                name: "AdvancedFinds");

            migrationBuilder.DropTable(
                name: "BankTransactions");

            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "BankAccountRegisterItemAmountDetails");

            migrationBuilder.DropTable(
                name: "QifMaps");

            migrationBuilder.DropTable(
                name: "BankAccountRegisterItems");

            migrationBuilder.DropTable(
                name: "BudgetItemSources");

            migrationBuilder.DropTable(
                name: "BudgetItems");

            migrationBuilder.DropTable(
                name: "BankAccounts");
        }
    }
}
