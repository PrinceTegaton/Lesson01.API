CREATE OR REPLACE VIEW View_Student
AS

SELECT *, CONCAT(FirstName, ' ', LastName) AS FullName, 1 AS Age
FROM Student