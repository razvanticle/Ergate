DECLARE @userId int

insert into Users(FirstName,LastName,Email)
values ('Razvan', 'Ticle', 'razvan.ticle@gmail.com')

select @userId=SCOPE_IDENTITY()

insert into Companies(Name,[Address],Email,Rating,OwnerId)
values('Company 1', 'tazlua 9','company@google.com',5,@userId),
('Company 2', 'tazlua 9','company@google.com',5,@userId),
('Company 3', 'tazlua 9','company@google.com',5,@userId)

select * from Users
select * from Companies

