CREATE DATABASE blog;

USE blog;

CREATE TABLE Users(
    UserId  INT PRIMARY KEY IDENTITY(1,1),
    UserName varchar(255),
    PasswordHash varchar(255),
    Salt varchar(255),
    Nonce varchar(255),
);
CREATE TABLE Posts(
    PostId      INT PRIMARY KEY IDENTITY(1,1),
    CreatedAt   DATETIME2 NOT NULL,
    Body        VARCHAR(255),
    UserId    INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (UserId) 
);

CREATE TABLE Comments(
    CommentId      INT PRIMARY KEY IDENTITY(1,1),
    CreatedAt   DATETIME2 NOT NULL,
    Body        VARCHAR(255),
    UserId    INT NOT NULL,
    PostId    INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (UserId),
    FOREIGN KEY (PostId) REFERENCES Posts (PostId) 
);

    
select 
	p.PostId, c.Body
from Posts p
	INNER JOIN Comments c on c.CommentId IN (
		select TOP 2 rc.commentid from Comments rc
      	where p.PostId = rc.postid
      	order by rc.commentid DESC
    )

    
use blog;
    select * from users

use blog;
update Users set Nonce = null