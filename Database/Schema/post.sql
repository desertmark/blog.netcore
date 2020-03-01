USE Blog;
CREATE TABLE Posts(
    PostId      INT PRIMARY KEY IDENTITY(1,1),
    CreatedAt   DATETIME2 NOT NULL,
    Body        VARCHAR(255),
    UserId    INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (UserId) 
);