CREATE OR ALTER PROCEDURE Pro_Login
    @username VARCHAR(255),
    @p_error_code INT OUTPUT,
    @p_error_message NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
      
        SET @p_error_code = 0;
        SET @p_error_message = N'';

        -- kiểm tra tồn tại user
        IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = @username)
        BEGIN
            SET @p_error_code = -1;
            SET @p_error_message = N'Tên đăng nhập không tồn tại';
            RETURN;
        END

        -- kiểm tra trạng thái (nếu có cột Status trong bảng Users)
        IF EXISTS (SELECT 1 FROM Users WHERE Username = @username AND Status <> N'Active')
        BEGIN
            SET @p_error_code = -2;
            SET @p_error_message = N'Tài khoản đã bị vô hiệu hóa';
            RETURN;
        END

        -- nếu không có lỗi thì select dữ liệu trả về
        SELECT 
            u.UserId,
            u.Username,
            u.FullName,
            u.Email,
            u.Status,
            r.RoleId,
            r.RoleName,
            f.FunctionId,
            f.FunctionName
        FROM Users u
        INNER JOIN Roles r ON u.RoleId = r.RoleId
        INNER JOIN RolePermissions rp ON r.RoleId = rp.RoleId
        INNER JOIN Functions f ON f.FunctionId = rp.FunctionId
        WHERE u.Username = @username;
    END TRY

    BEGIN CATCH
       
        SET @p_error_code = ERROR_NUMBER();
        SET @p_error_message = ERROR_MESSAGE();
    END CATCH
END
GO


CREATE OR ALTER PROCEDURE Pro_Register
    @userName   VARCHAR(255),
    @password   VARCHAR(255),
    @fullName   NVARCHAR(255),
    @email      NVARCHAR(255),
    @roleId     INT,
    @p_error_code INT OUTPUT,
    @p_error_message NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- reset lỗi mặc định
        SET @p_error_code = 0;
        SET @p_error_message = N'';

        -- kiểm tra username trùng
        IF EXISTS (SELECT 1 FROM Users WHERE Username = @userName)
        BEGIN
            SET @p_error_code = -1;
            SET @p_error_message = N'Tên đăng nhập đã tồn tại';
            RETURN;
        END

        -- kiểm tra email trùng
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @email)
        BEGIN
            SET @p_error_code = -2;
            SET @p_error_message = N'Email đã tồn tại';
            RETURN;
        END

        -- thực hiện insert
        INSERT INTO Users (Username, PasswordHash, FullName, Email, RoleId, CreatedAt)
        VALUES (@userName, @password, @fullName, @email, @roleId, GETDATE());
    END TRY

    BEGIN CATCH
        SET @p_error_code = ERROR_NUMBER();
        SET @p_error_message = ERROR_MESSAGE();
    END CATCH
END
GO
