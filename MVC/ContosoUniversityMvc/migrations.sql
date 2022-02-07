IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE TABLE [Course] (
        [CourseID] int NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Credits] int NOT NULL,
        CONSTRAINT [PK_Course] PRIMARY KEY ([CourseID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE TABLE [Student] (
        [ID] int NOT NULL IDENTITY,
        [LastName] nvarchar(max) NOT NULL,
        [FirstMidName] nvarchar(max) NOT NULL,
        [EnrollmentDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Student] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(128) NOT NULL,
        [ProviderKey] nvarchar(128) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(128) NOT NULL,
        [Name] nvarchar(128) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE TABLE [Enrollment] (
        [EnrollmentID] int NOT NULL IDENTITY,
        [CourseID] int NOT NULL,
        [StudentID] int NOT NULL,
        [Grade] int NULL,
        CONSTRAINT [PK_Enrollment] PRIMARY KEY ([EnrollmentID]),
        CONSTRAINT [FK_Enrollment_Course_CourseID] FOREIGN KEY ([CourseID]) REFERENCES [Course] ([CourseID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Enrollment_Student_StudentID] FOREIGN KEY ([StudentID]) REFERENCES [Student] ([ID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE INDEX [IX_Enrollment_CourseID] ON [Enrollment] ([CourseID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    CREATE INDEX [IX_Enrollment_StudentID] ON [Enrollment] ([StudentID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205064623_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220205064623_InitialCreate', N'6.0.1');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205075453_MaxLengthOnNames')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Student]') AND [c].[name] = N'LastName');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Student] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Student] ALTER COLUMN [LastName] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205075453_MaxLengthOnNames')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Student]') AND [c].[name] = N'FirstMidName');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Student] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Student] ALTER COLUMN [FirstMidName] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205075453_MaxLengthOnNames')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220205075453_MaxLengthOnNames', N'6.0.1');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205075642_ColumnFirstName')
BEGIN
    EXEC sp_rename N'[Student].[FirstMidName]', N'FirstName', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205075642_ColumnFirstName')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220205075642_ColumnFirstName', N'6.0.1');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205083123_ComplexDataModel')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Course]') AND [c].[name] = N'Title');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Course] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Course] ALTER COLUMN [Title] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205083123_ComplexDataModel')
BEGIN
    CREATE TABLE [Instructor] (
        [ID] int NOT NULL IDENTITY,
        [LastName] nvarchar(50) NOT NULL,
        [FirstName] nvarchar(50) NOT NULL,
        [HireDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Instructor] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205083123_ComplexDataModel')
BEGIN
    CREATE TABLE [CourseAssignment] (
        [CourseID] int NOT NULL,
        [InstructorID] int NOT NULL,
        CONSTRAINT [PK_CourseAssignment] PRIMARY KEY ([CourseID], [InstructorID]),
        CONSTRAINT [FK_CourseAssignment_Course_CourseID] FOREIGN KEY ([CourseID]) REFERENCES [Course] ([CourseID]) ON DELETE CASCADE,
        CONSTRAINT [FK_CourseAssignment_Instructor_InstructorID] FOREIGN KEY ([InstructorID]) REFERENCES [Instructor] ([ID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205083123_ComplexDataModel')
BEGIN
    CREATE TABLE [Department] (
        [DepartmentID] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NOT NULL,
        [Budget] money NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [InstructorID] int NULL,
        CONSTRAINT [PK_Department] PRIMARY KEY ([DepartmentID]),
        CONSTRAINT [FK_Department_Instructor_InstructorID] FOREIGN KEY ([InstructorID]) REFERENCES [Instructor] ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205083123_ComplexDataModel')
BEGIN
    INSERT INTO dbo.Department (Name, Budget, StartDate) VALUES ('Temp', 0.00, GETDATE())
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205083123_ComplexDataModel')
BEGIN
    ALTER TABLE [Course] ADD [DepartmentID] int NOT NULL DEFAULT 1;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205083123_ComplexDataModel')
BEGIN
    CREATE TABLE [OfficeAssignment] (
        [InstructorID] int NOT NULL,
        [Location] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_OfficeAssignment] PRIMARY KEY ([InstructorID]),
        CONSTRAINT [FK_OfficeAssignment_Instructor_InstructorID] FOREIGN KEY ([InstructorID]) REFERENCES [Instructor] ([ID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205083123_ComplexDataModel')
BEGIN
    CREATE INDEX [IX_Course_DepartmentID] ON [Course] ([DepartmentID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205083123_ComplexDataModel')
BEGIN
    CREATE INDEX [IX_CourseAssignment_InstructorID] ON [CourseAssignment] ([InstructorID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205083123_ComplexDataModel')
BEGIN
    CREATE INDEX [IX_Department_InstructorID] ON [Department] ([InstructorID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205083123_ComplexDataModel')
BEGIN
    ALTER TABLE [Course] ADD CONSTRAINT [FK_Course_Department_DepartmentID] FOREIGN KEY ([DepartmentID]) REFERENCES [Department] ([DepartmentID]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220205083123_ComplexDataModel')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220205083123_ComplexDataModel', N'6.0.1');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207023203_DepartmentRowVersion')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OfficeAssignment]') AND [c].[name] = N'Location');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [OfficeAssignment] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [OfficeAssignment] ALTER COLUMN [Location] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207023203_DepartmentRowVersion')
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Department]') AND [c].[name] = N'Name');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Department] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [Department] ALTER COLUMN [Name] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207023203_DepartmentRowVersion')
BEGIN
    ALTER TABLE [Department] ADD [RowVersion] rowversion NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207023203_DepartmentRowVersion')
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Course]') AND [c].[name] = N'Title');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Course] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [Course] ALTER COLUMN [Title] nvarchar(50) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207023203_DepartmentRowVersion')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220207023203_DepartmentRowVersion', N'6.0.1');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    CREATE TABLE [Person] (
        [ID] int NOT NULL IDENTITY,
        [LastName] nvarchar(50) NOT NULL,
        [FirstName] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_Person] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Person] ADD [Discriminator] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Person] ADD [OldId] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DROP INDEX [IX_Enrollment_StudentID] ON [Enrollment];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DROP INDEX [IX_Course_DepartmentID] ON [Course];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DROP INDEX [IX_CourseAssignment_InstructorID] ON [CourseAssignment];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DROP INDEX [IX_Department_InstructorID] ON [Department];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Enrollment] DROP CONSTRAINT [FK_Enrollment_Student_StudentID];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [OfficeAssignment] DROP CONSTRAINT [FK_OfficeAssignment_Instructor_InstructorID];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Department] DROP CONSTRAINT [FK_Department_Instructor_InstructorID];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [CourseAssignment] DROP CONSTRAINT [FK_CourseAssignment_Instructor_InstructorID];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Student] DROP CONSTRAINT [PK_Student];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Instructor] DROP CONSTRAINT [PK_Instructor];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    INSERT INTO dbo.Person (LastName, FirstName, Discriminator, OldId) SELECT LastName, FirstName, 'Student' AS Discriminator, ID AS OldId FROM dbo.Student
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    INSERT INTO dbo.Person (LastName, FirstName, Discriminator, OldId) SELECT LastName, FirstName, 'Instructor' AS Discriminator, ID AS OldId FROM dbo.Instructor
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    EXEC sp_rename N'[Student].[ID]', N'OldId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    EXEC sp_rename N'[Instructor].[ID]', N'OldId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Student] ADD [ID] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Instructor] ADD [ID] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    UPDATE dbo.Student SET ID = (SELECT ID FROM dbo.Person WHERE OldId = Student.OldId AND Discriminator = 'Student')
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    UPDATE dbo.Instructor SET ID = (SELECT ID FROM dbo.Person WHERE OldId = Instructor.OldId AND Discriminator = 'Instructor')
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Student]') AND [c].[name] = N'ID');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Student] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [Student] ALTER COLUMN [ID] int NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Instructor]') AND [c].[name] = N'ID');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Instructor] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Instructor] ALTER COLUMN [ID] int NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    UPDATE dbo.Enrollment SET StudentID = (SELECT ID FROM dbo.Person WHERE OldId = Enrollment.StudentID AND Discriminator = 'Student')
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    UPDATE dbo.OfficeAssignment SET InstructorID = (SELECT ID FROM dbo.Person WHERE OldId = OfficeAssignment.InstructorID AND Discriminator = 'Instructor')
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    UPDATE dbo.CourseAssignment SET InstructorID = (SELECT ID FROM dbo.Person WHERE OldId = CourseAssignment.InstructorID AND Discriminator = 'Instructor')
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    UPDATE dbo.Department SET InstructorID = (SELECT ID FROM dbo.Person WHERE OldId = Department.InstructorID AND Discriminator = 'Instructor') WHERE InstructorID IS NOT NULL
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Student] ADD CONSTRAINT [PK_Student] PRIMARY KEY ([ID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Instructor] ADD CONSTRAINT [PK_Instructor] PRIMARY KEY ([ID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Instructor] ADD CONSTRAINT [FK_Instructor_Person_ID] FOREIGN KEY ([ID]) REFERENCES [Person] ([ID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Student] ADD CONSTRAINT [FK_Student_Person_ID] FOREIGN KEY ([ID]) REFERENCES [Person] ([ID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Enrollment] ADD CONSTRAINT [FK_Enrollment_Student_StudentID] FOREIGN KEY ([StudentID]) REFERENCES [Student] ([ID]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [OfficeAssignment] ADD CONSTRAINT [FK_OfficeAssignment_Instructor_InstructorID] FOREIGN KEY ([InstructorID]) REFERENCES [Instructor] ([ID]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [Department] ADD CONSTRAINT [FK_Department_Instructor_InstructorID] FOREIGN KEY ([InstructorID]) REFERENCES [Instructor] ([ID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    ALTER TABLE [CourseAssignment] ADD CONSTRAINT [FK_CourseAssignment_Instructor_InstructorID] FOREIGN KEY ([InstructorID]) REFERENCES [Instructor] ([ID]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    CREATE INDEX [IX_Enrollment_StudentID] ON [Enrollment] ([StudentID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    CREATE INDEX [IX_Course_DepartmentID] ON [Course] ([DepartmentID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    CREATE INDEX [IX_CourseAssignment_InstructorID] ON [CourseAssignment] ([InstructorID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    CREATE INDEX [IX_Department_InstructorID] ON [Department] ([InstructorID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Student]') AND [c].[name] = N'FirstName');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Student] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [Student] DROP COLUMN [FirstName];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Student]') AND [c].[name] = N'LastName');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Student] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [Student] DROP COLUMN [LastName];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Student]') AND [c].[name] = N'OldId');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Student] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [Student] DROP COLUMN [OldId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Instructor]') AND [c].[name] = N'FirstName');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Instructor] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [Instructor] DROP COLUMN [FirstName];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Instructor]') AND [c].[name] = N'LastName');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Instructor] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [Instructor] DROP COLUMN [LastName];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Instructor]') AND [c].[name] = N'OldId');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Instructor] DROP CONSTRAINT [' + @var13 + '];');
    ALTER TABLE [Instructor] DROP COLUMN [OldId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Person]') AND [c].[name] = N'OldId');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Person] DROP CONSTRAINT [' + @var14 + '];');
    ALTER TABLE [Person] DROP COLUMN [OldId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Person]') AND [c].[name] = N'Discriminator');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Person] DROP CONSTRAINT [' + @var15 + '];');
    ALTER TABLE [Person] DROP COLUMN [Discriminator];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220207064305_Inheritance')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220207064305_Inheritance', N'6.0.1');
END;
GO

COMMIT;
GO

