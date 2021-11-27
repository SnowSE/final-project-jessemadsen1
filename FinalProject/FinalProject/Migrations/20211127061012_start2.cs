using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProject.Migrations
{
    public partial class start2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Topics_TopicID",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Channels_ChannelID",
                table: "Topics");

            migrationBuilder.RenameColumn(
                name: "ChannelID",
                table: "Topics",
                newName: "ChannelId");

            migrationBuilder.RenameIndex(
                name: "IX_Topics_ChannelID",
                table: "Topics",
                newName: "IX_Topics_ChannelId");

            migrationBuilder.RenameColumn(
                name: "TopicID",
                table: "Posts",
                newName: "TopicId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_TopicID",
                table: "Posts",
                newName: "IX_Posts_TopicId");

            migrationBuilder.AlterColumn<int>(
                name: "ChannelId",
                table: "Topics",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Topics_TopicId",
                table: "Posts",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Channels_ChannelId",
                table: "Topics",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Topics_TopicId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Channels_ChannelId",
                table: "Topics");

            migrationBuilder.RenameColumn(
                name: "ChannelId",
                table: "Topics",
                newName: "ChannelID");

            migrationBuilder.RenameIndex(
                name: "IX_Topics_ChannelId",
                table: "Topics",
                newName: "IX_Topics_ChannelID");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "Posts",
                newName: "TopicID");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_TopicId",
                table: "Posts",
                newName: "IX_Posts_TopicID");

            migrationBuilder.AlterColumn<int>(
                name: "ChannelID",
                table: "Topics",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "TopicID",
                table: "Posts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Topics_TopicID",
                table: "Posts",
                column: "TopicID",
                principalTable: "Topics",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Channels_ChannelID",
                table: "Topics",
                column: "ChannelID",
                principalTable: "Channels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
