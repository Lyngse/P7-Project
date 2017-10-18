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
    let socket = new WebSocket("ws://localhost:5000");
    socket.addEventListener('message', (event) => {
      console.log(event.data);
    });
    socket.addEventListener('open', () => {
      socket.send("hello from angular");
      
    });
    
  }


}
