<div align="center"><img  width="200" height="200" src="https://i.imgur.com/lJgIXDC.png"></div>
<h1 align="center">Geometric Farm</h1>

<h2>About</h2>
This project is an educational math video game developed in collaboration with Escuela Metropolitana, a top-ranked elementary school in Mexico City. The game is tailored to their curriculum standards and aims to engage students in learning various mathematical concepts through interactive gameplay mechanics.
The video game covers key math topics such as shape definitions, arithmetic operations, area calculations, and more. The content is designed to align with different grade levels, ensuring that students are presented with appropriate challenges based on their current level of understanding.
In addition to the game itself, the project includes a backend analytics system that tracks individual and cohort progress, storing data in a mySQL database. The collected data is then visualized through a web-based teacher dashboard, providing insightful representations such as leaderboards, success/failure rates, progress percentages, and maximum scores achieved by level.

<h2>Installation</h2>

### Prerequisites

Make sure you have the following prerequisites installed on your system:

- [Docker](https://www.docker.com/)
- [Git](https://git-scm.com/)

### Steps

1. **Clone the Repository**

   ```bash
   git clone https://github.com/Jose-AE/geometric-farm.git
   ```

2. **Navigate to the Project Directory**

   ```bash
   cd geometric-farm
   ```

3. **Run Containers**

   Use Docker Compose to spin up all the necessary containers:

   ```bash
   docker compose up
   ```

4. **Access Frontend**

   Once the containers are up and running, you can access the frontend of the application at:

   [http://localhost:3000](http://localhost:3000)

5. **Access Backend**

   The backend services will be available at:

   [http://localhost:5173](http://localhost:5173)

6. **Access MySQL Server**

   The MySQL server instance can be accessed at:

   localhost:3306

<h2>Copyright</h2>
This project is licensed under the terms of the GNU General Public v3.0 License. See <a href="LICENSE">license</a>
