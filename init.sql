CREATE DATABASE IF NOT EXISTS ecommerce;
use ecommerce;
CREATE TABLE IF NOT EXISTS Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL, -- In a real app, store hashes, not plain text!
    Role VARCHAR(50) NOT NULL
);

-- Insert dummy users for testing
INSERT INTO Users (Email, Password, Role) VALUES 
('admin@demo.com', '123456', 'Admin'),
('common@demo.com', '123456', 'Common');