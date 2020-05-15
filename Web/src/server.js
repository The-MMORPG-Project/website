const express = require('express')
const jwt = require('jsonwebtoken')

const app = express()

const bodyParser = require('body-parser')
app.use(bodyParser.json())
app.use(bodyParser.urlencoded({
  extended: true
}))

const mysql = require('mysql')

const db = mysql.createConnection({
  host: 'localhost',
  user: 'root',
  password: '123456'
})

db.connect((err) => {
  if (err) {
    throw err
  }
  console.log('MySQL connected...')
})

app.get('/api', (req, res) => {
  res.json({
    message: 'Welcome to the API'
  })
})

app.get('/api/createdb', (req, res) => {
  const sql = 'CREATE DATABASE database'
  db.query(sql, (err, result) => {
    if (err) throw err
    console.log('Database created...')
    res.send('Database created...')
  })
})

app.post('/api/posts', verifyToken, (req, res) => {
  jwt.verify(req.token, 'secretkey', (err, authData) => {
    if (err) {
      res.sendStatus(403)
    } else {
      res.json({
        message: 'Posted created...',
        authData
      })
    }
  })
})

app.post('/api/login', (req, res) => {
  const user = req.body
  jwt.sign({ user: user }, 'secretkey', (err, token) => {
    res.json({
      token: token
    })
  })
})

app.listen('3000', () => {
  console.clear()
  console.log('Listening on port 3000')
})

function verifyToken(req, res, next) {
  const bearerHeader = req.headers.authorization

  if (typeof bearerHeader !== 'undefined') {
    const bearer = bearerHeader.split(' ')
    const bearerToken = bearer[1]
    req.token = bearerToken
    next()
  } else {
    // Forbidden
    res.sendStatus(403)
  }
}
