using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginAndViewData.Migrations
{
    public partial class codefirst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Movie_Id = table.Column<int>(type: "int", nullable: false),
                    Movie_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Movie_Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentDetails",
                columns: table => new
                {
                    Student_Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mobile_No = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Blood_Group = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    File_Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDetails", x => x.Student_Id);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Subject_Id = table.Column<int>(type: "int", nullable: false),
                    Subject_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Subject_Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieStudent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Movie_Id = table.Column<int>(type: "int", nullable: false),
                    Student_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieStudent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieStudent_Movie",
                        column: x => x.Movie_Id,
                        principalTable: "Movie",
                        principalColumn: "Movie_Id");
                    table.ForeignKey(
                        name: "FK_MovieStudent_MovieStudent",
                        column: x => x.Student_Id,
                        principalTable: "StudentDetails",
                        principalColumn: "Student_Id");
                });

            migrationBuilder.CreateTable(
                name: "SubjectStudent",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_SubjectStudent_StudentDetails",
                        column: x => x.StudentId,
                        principalTable: "StudentDetails",
                        principalColumn: "Student_Id");
                    table.ForeignKey(
                        name: "FK_SubjectStudent_Subject",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Subject_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieStudent_Movie_Id",
                table: "MovieStudent",
                column: "Movie_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MovieStudent_Student_Id",
                table: "MovieStudent",
                column: "Student_Id");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectStudent_StudentId",
                table: "SubjectStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectStudent_SubjectId",
                table: "SubjectStudent",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieStudent");

            migrationBuilder.DropTable(
                name: "SubjectStudent");

            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.DropTable(
                name: "StudentDetails");

            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}
