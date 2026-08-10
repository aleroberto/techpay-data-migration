CREATE TABLE Customers (
    CustomerId INT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Document VARCHAR(20) NOT NULL,
    Email VARCHAR(150),
    Status VARCHAR(20) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Accounts (
    AccountId INT PRIMARY KEY,
    CustomerId INT NOT NULL,
    Type VARCHAR(30) NOT NULL,
    Balance DECIMAL(18,2) NOT NULL,
    Status VARCHAR(20) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Accounts_Customers
        FOREIGN KEY (CustomerId)
        REFERENCES Customers(CustomerId)
);

CREATE TABLE Transactions (
    TransactionId BIGINT PRIMARY KEY,
    AccountId INT NOT NULL,
    Type VARCHAR(30) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Status VARCHAR(20) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Transactions_Accounts
        FOREIGN KEY (AccountId)
        REFERENCES Accounts(AccountId)
);