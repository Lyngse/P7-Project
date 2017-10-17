var express = require('express');
var app = express();
var port = 3000;
var bodyParser = require('body-parser');
var dict = {};
app.use(bodyParser.urlencoded({extended:true}));
app.use(bodyParser.json());

app.use(function(req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    next();
  });

app.route('/host')
    .post(function(req, res) {
        var data = req.body;
        if(!data){
            res.send({error:"no data"});
        }
        console.log(req.ip);
        var ip = data.ip;
        if(!ip){
            res.send({error:"no ip!"});
        }

        do{
        var char1 = String.fromCharCode(97 + Math.floor(Math.random() * 26));
        var char2 = String.fromCharCode(97 + Math.floor(Math.random() * 26));
        var char3 = String.fromCharCode(97 + Math.floor(Math.random() * 26));
        var char4 = String.fromCharCode(97 + Math.floor(Math.random() * 26));
        var code = char1 + char2 + char3 + char4;  
        }while(dict[code]);

        dict[code] = ip;
        res.json({code: code});
    });

app.route('/connect')
    .post(function(req, res) {
        var data = req.body;
        if(!data){
            res.send({error:"no data"});
        }

        var code = data.code;

        if(!code)
            res.send({error: "no code bro!"});
        
        var ip = dict[code];

        if(!ip){
            res.send({error: "no match"});
        }

        res.json({ip: ip});
    });


app.listen(port);