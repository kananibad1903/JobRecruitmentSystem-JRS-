using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobRecruitmentSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddJobPostApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "JobPosts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "JobPosts");
        }
    }
}
