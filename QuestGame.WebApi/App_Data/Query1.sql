DECLARE  @roleName nvarchar(100)
set @roleName='admin'

--select *
--from AspNetRoles
--where Name='user'


--select *
--from AspNetUserRoles
--where RoleId=
--	(select Id
--	from AspNetRoles
--	where Name='user')


--select *
--from Quest
--where Quest.OwnerId=
--	(select UserId
--	from AspNetUserRoles
--	where RoleId=
--		(select Id
--		from AspNetRoles
--		where Name='admin')
--	)


--select u.UserId
--from AspNetUserRoles u
--inner join AspNetRoles r
--on r.Id=u.RoleId and r.Name='user'

select *
from Quests
where Quests.OwnerId in
		(select u.UserId
		from AspNetUserRoles u
		inner join AspNetRoles r
		on r.Id=u.RoleId and r.Name=@roleName)


