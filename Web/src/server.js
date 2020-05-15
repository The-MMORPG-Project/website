var express = require('express');
var jwt = require('jsonwebtoken');

var app = express();

app.get('/', function(req, res){
  res.send({"data": "test"})
})

app.get('/token', function(req, res){
  var token = jwt.sign({username:"ado"}, 'supersecret',{expiresIn: 120});
  res.send(token)
})

app.get('/api', function(req, res){
  var token = req.query.token;
  jwt.verify(token, 'supersecret', function(err, decoded){
    if(!err){
      var secrets = {"accountNumber" : "938291239","pin" : "11289","account" : "Finance"};
      res.json(secrets);
    } else {
      res.send(err);
    }
  })
})

app.listen('3000');

console.clear()
console.log('Listening on port 3000')