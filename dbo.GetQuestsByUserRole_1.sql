CREATE PROCEDURE [dbo].[GetQuestsByUserRole]
	@roleName nvarchar(100)
AS
	SELECT * 
	FROM Quest
	WHERE Quest.OwnerId IN
		(SELECT u.UserId
		FROM AspNetUserRoles u
		INNER JOIN AspNetRoles
		ON r.Id=u.RoleId AND r.name=@roleName)
GO