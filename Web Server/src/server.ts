import 'dotenv/config'
import express from 'express'
import * as jwt from 'jsonwebtoken'
import * as bcrypt from 'bcrypt'
import * as MySQL from 'promise-mysql'
import * as bodyParser from 'body-parser'
import { config } from './config'
import { StatusCode } from './statuscode'

const init = async () => {
  console.clear()
  
  const app = express()

  app.use(bodyParser.json())
  app.use(bodyParser.urlencoded({ extended: true }))
  app.use(express.static('src/public'))

  let db: MySQL.Connection;

  if (config.online) {
    db = await MySQL.createConnection({
      host: process.env.DB_HOST,
      user: process.env.DB_USER,
      password: process.env.DB_PASSWORD
    })

    const databases = await db.query("SHOW DATABASES LIKE 'database'")
    if (databases.length === 0) {
      await db.query("CREATE DATABASE `database`")
      await db.query("USE `database`")
      await db.query("CREATE TABLE users (id INT(11) PRIMARY KEY AUTO_INCREMENT, username VARCHAR(20), password VARCHAR(60))")
      console.log("Created database")
    }

    await db.query("USE `database`")

    console.log("Connected to MySQL")
  } else {
    console.log("Running in offline mode")
  }

  app.listen(config.server.port)
  console.log(`Node server is listening on port ${config.server.port}`)

  // --------------------------------------------------------------------
  app.get("/api", (req, res) => {
    res.json({ message: "Welcome to the web server API" })
  })

  app.get("/api/releases/:platform/:version", (req, res) => {
    const platform = req.params.platform
    const version = req.params.version
    const file = `./src/releases/${platform}/${version}`
    res.download(file)
  })

  app.post("/api/posts", verifyToken, (req, res) => {
    // (req as { [ key: string ]:any }).token use to be just req.token
    jwt.verify((req as { [key: string]: any }).token, "secretkey", (err, authData) => {
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

  app.post("/api/register", async (req, res) => {
    const user = req.body
    const name = user.username
    const pass = user.password

    console.log(user)

    if (name == undefined || pass == undefined) 
    {
      return
    }

    const results = await db.query("SELECT * FROM `users` WHERE username = ?", [name])

    if (results.length != 0) {
      // User already exists in database
      res.json({ status: StatusCode.REGISTER_ACCOUNT_ALREADY_EXISTS })
      //console.log(`User tried to register the account '${name}' but it already exists`)
      return
    }

    if (name.length < config.username.min_length || name.length > config.username.max_length) {
      // Invalid username
      res.json({ status: StatusCode.REGISTER_USERNAME_INVALID })
      //console.log(`User tried to register the account '${name}' but their username was invalid`)
      return
    }

    if (pass.length < config.password.min_length || pass.length > config.password.max_length) {
      // Invalid password
      res.json({ status: StatusCode.REGISTER_PASSWORD_INVALID })
      //console.log(`User tried to register the account '${name}' but their password was invalid`)
      return
    }

    // Register user to database and encrpyt their password
    const saltRounds = 12 // 10 is weak, 12 is stronger
    bcrypt.hash(pass, saltRounds, async (err, hash) => {
      if (err) throw err
      await db.query("INSERT INTO users (username, password) VALUES (?, ?)", [name, hash])
    })

    res.json({ status: StatusCode.REGISTER_SUCCESS })
    console.log(`User registered account '${name}' to database`)
  })

  app.post("/api/login", async (req, res) => {
    const user = req.body
    const name = user.username
    const pass = user.password

    const results = await db.query("SELECT * FROM `users` WHERE username = ?", [name])

    if (results.length == 0) {
      res.json({ status: StatusCode.LOGIN_DOESNT_EXIST })
      //console.log(`User tried to login to the account '${name}' but it does not exist`)
      return
    }

    bcrypt.compare(pass, results[0].password, (err, result) => {
      if (err) throw err
      if (!result) {
        res.json({ status: StatusCode.LOGIN_WRONG_PASSWORD })
        console.log(`User failed to log into account '${name}'`)
        return
      }

      jwt.sign({ user: user }, "secretkey", (err, token) => {
        if (err) throw err
        res.json({
          status: StatusCode.LOGIN_SUCCESS,
          token: token
        })
      })

      console.log(`User logged into account '${name}'`)
    })
  })

  // Middleware
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

  // Commented out because db.query("SELECT 1") logs 'test' to console every time
  // its executed which is really strange
  //setInterval(keepConnectionsAlive, 1000 * 10, db)
}

init()

function keepConnectionsAlive(db) {
  db.query("SELECT 1")
}