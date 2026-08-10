INSERT INTO Customers
(CustomerId, Name, Document, Email, Address, Status)
VALUES
(1, 'Joao Silva', '11111111111', 'joao@email.com', 'Rua A, 100', 'ACTIVE'),
(2, 'Maria Souza', '22222222222', 'maria@email.com', 'Rua B, 200', 'ACTIVE'),
(3, 'Carlos Lima', '33333333333', 'carlos@email.com', 'Rua C, 300', 'ACTIVE');

INSERT INTO Accounts
(AccountId, CustomerId, AccountType, Balance, Status, UpdatedAt)
VALUES
(101, 1, 'CHECKING', 1500.00, 'ACTIVE', GETDATE()),
(102, 2, 'SAVINGS', 2500.00, 'ACTIVE', GETDATE()),
(103, 3, 'CHECKING', 800.00, 'ACTIVE', GETDATE());

INSERT INTO Transactions
(TransactionId, AccountId, Type, Amount, TransactionDate, Status)
VALUES
(1001, 101, 'CREDIT', 1500.00, GETDATE(), 'COMPLETED'),
(1002, 102, 'CREDIT', 2500.00, GETDATE(), 'COMPLETED'),
(1003, 103, 'DEBIT', 200.00, GETDATE(), 'COMPLETED');