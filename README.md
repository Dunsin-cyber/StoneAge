# 🪨 Stone Age Farm

Welcome to **Stone Age Farm**, a unique play-to-earn farming game set in a primitive world where players start with a piece of land and some carved stones. As the game evolves, players can purchase equipment from the future, buy additional lands, and monopolize their farming empire. The game integrates **TON** and **AEON Payment Protocol** for in-game purchases, land sales, and NFT minting to verify ownership of lands and inventions.

![Stone Age Farm Banner]()

## 🛠️ Tech Stack

- **Game Engine**: Unity
- **Backend**: Node.js, Express, MongoDB
- **Blockchain Integration**: TON, AEON Payment Protocol
- **Real-time Communication**: Socket.IO
- **Payment Integration**: TON, AEON
- **Deployment**

## 📖 Table of Contents

1. [Features](#features)
2. [Getting Started](#getting-started)
3. [Backend API](#backend-api)
4. [Socket.IO Events](#socketio-events)
5. [Payment Integration](#payment-integration)
6. [Contributing](#contributing)
7. [License](#license)
8. [Contact](#contact)

## ✨ Features

- **Farming Gameplay**: Start with a small piece of land, plant crops, harvest, and sell for profit.
- **In-Game Purchases**: Buy advanced tools and equipment from the future using crypto.
- **Land Ownership**: Acquire new lands, sell to other players or the government, and transfer ownership.
- **NFT Minting**: Mint NFTs for proof of land ownership.
- **Real-Time Multiplayer**: Social features like chat, trade, and land sales using Socket.IO.
- **Blockchain Integration**: Seamless integration with TON and AEON for payments and transactions.

## 📚 Getting Started

1. **Player Registration**: Users register using their wallet address.
2. **Starting the Game**: Players are given a piece of land and basic tools.
3. **Farming**: Plant crops, wait for harvest, and sell produce for in-game currency.
4. **Upgrades**: Use AEON or TON to buy advanced tools from the future.
5. **Land Ownership**: Buy additional lands, trade with players, or sell to the government.
6. **NFT Minting**: Transfer land ownership and mint NFT proof.

## 🛠️ Backend API

The backend is built with **Node.js**, **Express**, and **MongoDB**. Here are some key endpoints:

### User Endpoints

| Method | Endpoint           | Description           |
|--------|--------------------|-----------------------|
| POST   | `/api/users/register` | Register a new user    |
| GET    | `/api/users/profile`  | Get user profile       |
| POST   | `/api/users/login`    | User login (JWT-based) |

### Game Endpoints

| Method | Endpoint           | Description                   |
|--------|--------------------|-------------------------------|
| GET    | `/api/game/start`  | Start the game session        |
| POST   | `/api/game/harvest`| Harvest crops                 |
| POST   | `/api/game/buyLand`| Buy additional land           |

### NFT Endpoints

| Method | Endpoint           | Description                   |
|--------|--------------------|-------------------------------|
| POST   | `/api/nft/mint`    | Mint NFT for land ownership   |
| GET    | `/api/nft/verify`  | Verify NFT ownership          |

## 🔄 Socket.IO Events

The game uses **Socket.IO** for real-time communication. Below are some key events:

| Event             | Description                       |
|-------------------|-----------------------------------|
| `userOnline`      | Notify server of user online status |
| `sendMessage`     | Send a chat message               |
| `landSale`        | Notify players of a land sale     |
| `harvestUpdate`   | Update players about crop harvest |

### Example Unity Client Code

```csharp
using SocketIOClient;

SocketIO client = new SocketIO("http://localhost:5000");

await client.ConnectAsync();
await client.EmitAsync("userOnline", "walletAddress123");

client.On("landSale", response => {
    Debug.Log("Land Sale: " + response.ToString());
});
```

## 💳 Payment Integration
The game uses AEON and TON for seamless blockchain payments.

Deposit Funds: Players can deposit AEON or TON to buy equipment and land.
Withdrawal: Players can withdraw earnings to their crypto wallet.
NFT Minting: Land ownership is verified and minted as NFTs.
Payment Services
Check the payment integration code in services/tonService.js and services/aeonService.js.

## 🤝 Contributing
We welcome contributions from the community! Please follow these steps:

Fork the repository.
Create a new branch (git checkout -b feature-branch).
Make your changes and commit (git commit -m 'Add new feature').
Push to the branch (git push origin feature-branch).
Open a Pull Request.
Please read our Contributing Guidelines for more details.

## 📄 License
This project is licensed under the MIT License - see the LICENSE file for details.