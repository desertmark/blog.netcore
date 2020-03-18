USE blog;

-- INSERT INTO Users (UserName, PasswordHash) values ('admin', 'HashdeAbcd1234');
INSERT INTO Posts (CreatedAt, Body, UserId) values (GETDATE(), 'Post 1', 1);
INSERT INTO Posts (CreatedAt, Body, UserId) values (GETDATE(), 'Post 2', 1);
INSERT INTO Comments (CreatedAt, Body, UserId, PostId) values (GETDATE(), 'Comment 1', 1, 1);
INSERT INTO Comments (CreatedAt, Body, UserId, PostId) values (GETDATE(), 'Comment 1', 1, 1);
INSERT INTO Comments (CreatedAt, Body, UserId, PostId) values (GETDATE(), 'Comment 1', 1, 1);
INSERT INTO Comments (CreatedAt, Body, UserId, PostId) values (GETDATE(), 'Comment 1', 1, 2);
INSERT INTO Comments (CreatedAt, Body, UserId, PostId) values (GETDATE(), 'Comment 1', 1, 2);
INSERT INTO Comments (CreatedAt, Body, UserId, PostId) values (GETDATE(), 'Comment 1', 1, 2);
