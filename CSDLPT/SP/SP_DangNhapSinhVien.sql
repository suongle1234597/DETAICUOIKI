ALTER PROC [dbo].[SP_DangNhapSinhVien]
@TENLOGIN NVARCHAR (50),
@MASV NVARCHAR (50)
AS
DECLARE @TENUSER NVARCHAR(50), @HOTEN NVARCHAR(40)
SELECT @TENUSER=NAME FROM sys.sysusers WHERE sid = SUSER_SID(@TENLOGIN)

SET @HOTEN = (SELECT HO + ' '+ TEN FROM SINHVIEN  WHERE MASV = @MASV)
IF @HOTEN IS NULL
BEGIN 
	SET @HOTEN = 'NULL'
END

 SELECT USERNAME = @MASV, 
  HOTEN = @HOTEN,
   TENNHOM= NAME
   FROM sys.sysusers 
   WHERE UID = (SELECT GROUPUID 
                 FROM SYS.SYSMEMBERS 
                   WHERE MEMBERUID= (SELECT UID FROM sys.sysusers 
                                      WHERE NAME=@TENUSER))