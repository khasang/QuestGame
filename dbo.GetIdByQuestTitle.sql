CREATE PROCEDURE [dbo].[GetIdByQuestTitle]
	@title varchar(100)
AS
	SELECT Id 
	FROM Quest
	WHERE Quest.title = @title