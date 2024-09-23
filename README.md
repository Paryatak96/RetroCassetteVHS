RetroCassetteVHS

RetroCassetteVHS is a web application designed to manage VHS cassette rentals. It allows users to browse, rent, and manage VHS cassettes, while administrators can manage users' accounts and wallets, track rental histories, and perform administrative tasks. This project follows a modular structure using the repository and service patterns to improve maintainability, scalability, and testability.



Features:

-User Registration and Login: Users can register, log in, and manage their accounts.

-Browse Cassettes: Users can browse the available collection of VHS cassettes, including descriptions, titles, and availability.

-Rent Cassettes: Users can rent a cassette, and the system ensures the user has sufficient wallet balance.

-Wallet Management: Users can add funds to their wallet. Administrators can update user balances.

-Admin Panel: Administrators can manage users, view their wallet balances, and modify user details.

-SendGrid Email Integration: Confirmation emails and notifications are sent using SendGrid.



Technology Stack:

Backend: ASP.NET Core

Frontend: Razor Pages, HTML, CSS

Database: Entity Framework Core (SQL Server)

Authentication: ASP.NET Identity

Email Service: SendGrid API

Design Pattern: Repository and Service pattern for data access and business logic separation.
