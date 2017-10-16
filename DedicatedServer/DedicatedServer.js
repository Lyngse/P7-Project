var express = require('express');
var app = express();
var port = 3000;
var bodyParser = require('body-parser');
var dict = {};
app.use(bodyParser.urlencoded({extended:true}));
app.use(bodyParser.json());

app.route('/host')
    .post(function(req, res) {
        var data = req.body;
        if(!data){
            res.send("no data");
        }

        var ip = data.ip;
        if(!ip){
            res.send("no ip!");
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
            res.send("no data");
        }

        var code = data.code;

        if(!code)
            res.send("no code bro!");
        
        var ip = dict[code];

        if(!ip){
            res.send("no match");
        }

        res.json({ip: ip});
    });


app.listen(port);