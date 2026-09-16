# Blazor Beams!
### A Demo Application for Blazor, The best/worst pun-focused social media application ever. ###

- Blazor WebAssembly Client
- Hosted in ASP.Net core, with a Web Api backend
- Cookie-based accounts: register with a username and password, then sign in
- SQL Server Database
- EF Core
- dotnet 10

## Accounts and sign in

Rays, prisms, and frequencies are created by signed-in users:

- **Create an account** at `/register`. Usernames are unique (3-32 characters) and passwords must be at least 8 characters.
- **Sign in** at `/login`, and change your password from `/settings`.
- Passwords are stored as salted PBKDF2 hashes (ASP.NET Core `PasswordHasher`), never in plain text.
- The session is held in an `HttpOnly` authentication cookie, and the server always takes the acting user from that cookie rather than from the request body.
- Reading frequencies and rays stays open to anonymous visitors.

Accounts created before passwords existed have no password and cannot sign in; register a new account instead.

Develop, Build and Run in a container locally (with docker desktop and the [Remote - Containers](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers) extension or in [Codespaces](https://github.com/features/codespaces) - Now prebuilt!
