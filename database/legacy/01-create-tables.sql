CREATE TABLE Customers (
    CustomerId INT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Document VARCHAR(20) NOT NULL,
    Email VARCHAR(150),
    Address VARCHAR(200),
    Status VARCHAR(20) NOT NULL
);

CREATE TABLE Accounts (
    AccountId INT PRIMARY KEY,
    CustomerId INT NOT NULL,
    AccountType VARCHAR(30) NOT NULL,
    Balance DECIMAL(18,2) NOT NULL,
    Status VARCHAR(20) NOT NULL,
    UpdatedAt DATETIME2 NOT NULL,

    CONSTRAINT FK_Accounts_Customers
        FOREIGN KEY (CustomerId)
        REFERENCES Customers(CustomerId)
);

CREATE TABLE Transactions (
    TransactionId BIGINT PRIMARY KEY,
    AccountId INT NOT NULL,
    Type VARCHAR(30) NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    TransactionDate DATETIME2 NOT NULL,
    Status VARCHAR(20) NOT NULL,

    CONSTRAINT FK_Transactions_Accounts
        FOREIGN KEY (AccountId)
        REFERENCES Accounts(AccountId)
);