USE Blog;
CREATE TABLE Users(
    UserId  INT PRIMARY KEY IDENTITY(1,1),
    UserName varchar(255),
    PasswordHash varchar(255),
);