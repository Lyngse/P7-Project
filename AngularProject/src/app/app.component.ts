import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';

  constructor(){
    console.log("hey");
    let socket = new WebSocket("ws://192.168.0.100:5000");
    socket.addEventListener('message', (event) => {
      console.log(event.data);
    });
    socket.addEventListener('open', () => {
      socket.send(
        JSON.stringify({type: "message", message: "hello from angular"})
      );
      
    });
    
  }


}
