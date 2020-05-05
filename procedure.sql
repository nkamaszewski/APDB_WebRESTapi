CREATE PROCEDURE PromoteStudents @Studies NVARCHAR(100), @Semester INT
AS
BEGIN
	SET XACT_ABORT ON;
	BEGIN TRAN
	DECLARE @IdStudy INT = (SELECT IdStudy FROM Studies WHERE Name=@Studies);

	IF @IdStudy IS NOT NULL
	BEGIN
		
	DECLARE @IdEnrollment INT = (SELECT IdEnrollment FROM Enrollment WHERE Enrollment.Semester = @Semester+1 AND Enrollment.IdStudy = @IdStudy AND Enrollment.StartDate = (SELECT MAX(StartDate) FROM Enrollment WHERE Enrollment.Semester = @Semester+1  AND Enrollment.IdStudy = @IdStudy));
	IF @IdEnrollment IS NULL
	BEGIN
		SET @IdEnrollment = (SELECT MAX(IdEnrollment) FROM Enrollment);
		INSERT INTO Enrollment VALUES (@IdEnrollment+1, @Semester+1, @IdStudy, '2020-10-01');
		SET @IdEnrollment=@IdEnrollment+1;
	END
	
	DECLARE @PrevIdEnrollment INT = (SELECT IdEnrollment FROM enrollment WHERE IdStudy=@IdStudy AND Semester=@Semester AND Enrollment.StartDate = (SELECT MAX(StartDate) FROM Enrollment WHERE Enrollment.Semester = @Semester  AND Enrollment.IdStudy = @IdStudy));
	IF @PrevIdEnrollment IS NOT NULL
	BEGIN
			UPDATE Student SET IdEnrollment=@IdEnrollment WHERE IdEnrollment=@PrevIdEnrollment;
	END

	END
	COMMIT
END