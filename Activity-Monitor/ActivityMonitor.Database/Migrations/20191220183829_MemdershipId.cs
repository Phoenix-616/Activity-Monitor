using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityMonitor.Database.Migrations
{
    public partial class MemdershipId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Memberships_MembershipId",
                table: "Issues");

            migrationBuilder.AlterColumn<int>(
                name: "MembershipId",
                table: "Issues",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Memberships_MembershipId",
                table: "Issues",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Memberships_MembershipId",
                table: "Issues");

            migrationBuilder.AlterColumn<int>(
                name: "MembershipId",
                table: "Issues",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Memberships_MembershipId",
                table: "Issues",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
