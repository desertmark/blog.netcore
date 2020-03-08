USE blog;
INSERT INTO Users (UserName, PasswordHash) values ('admin', 'HashdeAbcd1234');
INSERT INTO Posts (CreatedAt, Body, UserId) values (GETDATE(), 'My First Test Post!', 1);