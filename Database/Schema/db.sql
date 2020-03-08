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
