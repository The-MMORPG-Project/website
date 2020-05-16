const express = require("express")
const jwt = require("jsonwebtoken")
const bcrypt = require("bcrypt")

const app = express()

const bodyParser = require("body-parser")
app.use(bodyParser.json())
app.use(bodyParser.urlencoded({
  extended: true
}))

const { MySQL } = require("mysql-promisify")

const db = new MySQL({
  host: "localhost",
  user: "root",
  password: "nimda"
})

app.get("/api", (req, res) => {
  res.json({
    message: "Welcome to the API"
  })
})

app.post("/api/posts", verifyToken, (req, res) => {
  jwt.verify(req.token, "secretkey", (err, authData) => {
    if (err) {
      res.sendStatus(403)
    } else {
      res.json({
        message: "Posted created...",
        authData
      })
    }
  })
})

enum StatusCode {
  REGISTER_SUCCESS,
  REGISTER_ACCOUNT_ALREADY_EXISTS,
  REGISTER_PASSWORD_TOO_LONG,
  REGISTER_PASSWORD_TOO_SHORT,
  REGISTER_USERNAME_TOO_LONG,
  REGISTER_USERNAME_TOO_SHORT
}

app.post("/api/register", async (req, res) => {
  const user = req.body
  console.log(user)
  const { result } = await db.query({ sql: "SELECT * FROM `users` WHERE username = :user", params: { user: user.name } })
  if (result === undefined) {
    const saltRounds = 12
    bcrypt.hash("123", saltRounds, async (err, hash) => {
      if (err) throw err
      await db.query({sql: "INSERT INTO users (username, password) VALUES (:username, :password)", params: { username: user.name, password: hash}})
    })

    res.json({
      status: StatusCode.REGISTER_SUCCESS
    })
  } else {
    res.json({
      status: StatusCode.REGISTER_ACCOUNT_ALREADY_EXISTS
    })
  }
})

app.post("/api/login", (req, res) => {
  const user = req.body
  jwt.sign({ user: user }, "secretkey", (err, token) => {
    res.json({
      token: token
    })
  })
})

function verifyToken(req, res, next) {
  const bearerHeader = req.headers.authorization

  if (typeof bearerHeader !== "undefined") {
    const bearer = bearerHeader.split(" ")
    const bearerToken = bearer[1]
    req.token = bearerToken
    next()
  } else {
    // Forbidden
    res.sendStatus(403)
  }
}

(async () => {
  console.clear()
  console.log("Connected to MySQL")

  const { results } = await db.query({ sql: "SHOW DATABASES LIKE 'database'" })
  if (results.length === 0) {
    await db.query({ sql: "CREATE DATABASE `database`" })
    await db.query({ sql: "USE `database`" })
    await db.query({ sql: "CREATE TABLE users (id INT(11) PRIMARY KEY AUTO_INCREMENT, username VARCHAR(20), password VARCHAR(60))" })
    console.log("Created database")
  }

  await db.query({ sql: "USE `database`" })

  app.listen("3000", () => {
    console.log("Node server is listening on port 3000")
  })
})()