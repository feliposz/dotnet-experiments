using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContosoUniversityMvc.Migrations
{
    public partial class Inheritance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.ID);
                });

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Person",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldId",
                table: "Person",
                nullable: true);

            // Drop indices, PKs and FKs that will be updated

            migrationBuilder.DropIndex("IX_Enrollment_StudentID", "Enrollment");
            migrationBuilder.DropIndex("IX_Course_DepartmentID", "Course");
            migrationBuilder.DropIndex("IX_CourseAssignment_InstructorID", "CourseAssignment");
            migrationBuilder.DropIndex("IX_Department_InstructorID", "Department");

            migrationBuilder.DropForeignKey("FK_Enrollment_Student_StudentID", "Enrollment");
            migrationBuilder.DropForeignKey("FK_OfficeAssignment_Instructor_InstructorID", "OfficeAssignment");
            migrationBuilder.DropForeignKey("FK_Department_Instructor_InstructorID", "Department");
            migrationBuilder.DropForeignKey("FK_CourseAssignment_Instructor_InstructorID", "CourseAssignment");

            migrationBuilder.DropPrimaryKey("PK_Student", "Student");
            migrationBuilder.DropPrimaryKey("PK_Instructor", "Instructor");

            // Insert data from original tables onto new table

            migrationBuilder.Sql(
                "INSERT INTO dbo.Person (LastName, FirstName, Discriminator, OldId) " +
                "SELECT LastName, FirstName, 'Student' AS Discriminator, ID AS OldId FROM dbo.Student");

            migrationBuilder.Sql(
                "INSERT INTO dbo.Person (LastName, FirstName, Discriminator, OldId) " +
                "SELECT LastName, FirstName, 'Instructor' AS Discriminator, ID AS OldId FROM dbo.Instructor");

            // Update IDs to reflect new ID of Person

            migrationBuilder.RenameColumn("ID", "Student", "OldId");
            migrationBuilder.RenameColumn("ID", "Instructor", "OldId");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Student",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Instructor",
                nullable: true);

            migrationBuilder.Sql(
                "UPDATE dbo.Student SET ID = " +
                "(SELECT ID FROM dbo.Person WHERE OldId = Student.OldId AND Discriminator = 'Student')"
            );

            migrationBuilder.Sql(
                "UPDATE dbo.Instructor SET ID = " +
                "(SELECT ID FROM dbo.Person WHERE OldId = Instructor.OldId AND Discriminator = 'Instructor')"
            );

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Student",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Instructor",
                nullable: false);

            // Update IDs from tables that reference Student and Instructor

            migrationBuilder.Sql(
                "UPDATE dbo.Enrollment SET StudentID = " +
                "(SELECT ID FROM dbo.Person WHERE OldId = Enrollment.StudentID AND Discriminator = 'Student')"
            );

            migrationBuilder.Sql(
                "UPDATE dbo.OfficeAssignment SET InstructorID = " +
                "(SELECT ID FROM dbo.Person WHERE OldId = OfficeAssignment.InstructorID AND Discriminator = 'Instructor')"
            );

            migrationBuilder.Sql(
                "UPDATE dbo.CourseAssignment SET InstructorID = " +
                "(SELECT ID FROM dbo.Person WHERE OldId = CourseAssignment.InstructorID AND Discriminator = 'Instructor')"
            );

            migrationBuilder.Sql(
                "UPDATE dbo.Department SET InstructorID = " +
                "(SELECT ID FROM dbo.Person WHERE OldId = Department.InstructorID AND Discriminator = 'Instructor') " +
                "WHERE InstructorID IS NOT NULL"
            );

            // Recreate primary keys

            migrationBuilder.AddPrimaryKey("PK_Student", "Student", "ID");
            migrationBuilder.AddPrimaryKey("PK_Instructor", "Instructor", "ID");

            // New foreign keys

            migrationBuilder.AddForeignKey(
                name: "FK_Instructor_Person_ID",
                table: "Instructor",
                column: "ID",
                principalTable: "Person",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Person_ID",
                table: "Student",
                column: "ID",
                principalTable: "Person",
                principalColumn: "ID");

            // Recreate foreign keys

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Student_StudentID",
                table: "Enrollment",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeAssignment_Instructor_InstructorID",
                table: "OfficeAssignment",
                column: "InstructorID",
                principalTable: "Instructor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Instructor_InstructorID",
                table: "Department",
                column: "InstructorID",
                principalTable: "Instructor",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignment_Instructor_InstructorID",
                table: "CourseAssignment",
                column: "InstructorID",
                principalTable: "Instructor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            // Recreate indices

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_StudentID",
                table: "Enrollment",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Course_DepartmentID",
                table: "Course",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAssignment_InstructorID",
                table: "CourseAssignment",
                column: "InstructorID");

            migrationBuilder.CreateIndex(
                name: "IX_Department_InstructorID",
                table: "Department",
                column: "InstructorID");



            // CLEANUP: Drop column from old tables

            migrationBuilder.DropColumn("FirstName", "Student");
            migrationBuilder.DropColumn("LastName", "Student");
            migrationBuilder.DropColumn("OldId", "Student");
            migrationBuilder.DropColumn("FirstName", "Instructor");
            migrationBuilder.DropColumn("LastName", "Instructor");
            migrationBuilder.DropColumn("OldId", "Instructor");
            migrationBuilder.DropColumn("OldId", "Person");
            migrationBuilder.DropColumn("Discriminator", "Person");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            throw new NotImplementedException();
        }

    }
}
