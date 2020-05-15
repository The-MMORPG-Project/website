var express = require('express')
var jwt = require('jsonwebtoken')

var app = express()

var bodyParser = require('body-parser')
app.use(bodyParser.json())
app.use(bodyParser.urlencoded({
  extended: true
}))

app.get('/api', (req, res) => {
  res.json({
    message: 'Welcome to the API'
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
  var user = req.body
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
