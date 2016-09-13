CREATE PROCEDURE [dbo].[GetQuestsByUserRole]
	@roleName nvarchar(100)
AS
	SELECT *
	FROM Quests
	WHERE Quests.OwnerId IN
			(SELECT u.UserId
			FROM AspNetUserRoles u
			INNER JOIN AspNetRoles r
			ON r.Id=u.RoleId AND r.Name=@roleName)
GO