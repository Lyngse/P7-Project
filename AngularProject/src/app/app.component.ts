import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  private headers = new HttpHeaders().set('Content-Type', 'application/json');

  connectionCode: string = "";
  connectionUrl: string = "http://192.168.0.104:3000";

  constructor(private http: HttpClient){
    /*console.log("hey");
    let socket = new WebSocket("ws://localhost:5000");
    socket.addEventListener('message', (event) => {
      console.log(event.data);
    });
    socket.addEventListener('open', () => {
      socket.send(
        JSON.stringify({type: "message", message: "hello from angular"})
      );
      
    });*/
    
  }

  connect() {
    this.http
    .post(this.connectionUrl + "/connect", JSON.stringify({code: this.connectionCode}), {
      headers: this.headers
    })
    .subscribe(data => { console.log(data); })
  }
}
