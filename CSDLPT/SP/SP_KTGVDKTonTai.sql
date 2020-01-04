CREATE PROC SP_KTGVDKTonTai
	@MAMH NCHAR(8), 
	@MALOP NCHAR(8),
	@LAN SMALLINT
AS
BEGIN
DECLARE @RESULT INT
SET @RESULT = 0
IF EXISTS (SELECT * FROM GIAOVIEN_DANGKY WHERE @MAMH = MAMH AND @MALOP = MALOP AND @LAN = LAN)
BEGIN
	SET @RESULT = 1
END

ELSE IF EXISTS (SELECT * FROM LINK1.TRACNGHIEM.dbo.GIAOVIEN_DANGKY WHERE @MAMH = MAMH AND @MALOP = MALOP AND @LAN = LAN)
BEGIN
	SET @RESULT = 1
END
SELECT @RESULT AS 'RESULT'
END

