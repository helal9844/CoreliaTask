import { Component } from '@angular/core';
import { IEmployee } from 'src/Shared/Interfaces/IEmployee';
import { EmployeeServiceService } from 'src/Shared/Services/employee-service.service';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  private hubConnection: any;
  title = 'Corelia';
  AllEmployee: IEmployee[] = [];
  constructor(
    private employeeServiceService: EmployeeServiceService,
    private toastr: ToastrService
  ) {}
  ngOnInit() {
    this.employeeServiceService.GetAll().subscribe((data: any) => {
      this.AllEmployee = data;
      console.log(this.AllEmployee);
    });
    this.initializeSignalR();
  }
  private initializeSignalR() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7160/notificationHub', {
        withCredentials: true,
      })
      .build();

    this.hubConnection.start().then(() => {
      console.log('Hub connection started');

      // Subscribe to the 'ReceiveNotification' event
      this.hubConnection
        .on('ReceiveNotification', (message: string) => {
          // Handle the received notification (e.g., update the employee list)
          console.log('Received Notification:', message);
          this.refreshEmployeeList();
          this.showNotification(message);
        })
        .catch((error: any) => {
          console.error('Error starting hub connection:', error);
        });
    });
  }
  private refreshEmployeeList() {
    this.employeeServiceService.GetAll().subscribe((data: any) => {
      this.AllEmployee = data;
    });
  }
  private showNotification(message: string) {
    let operation = '';

    if (message.includes('added')) {
      operation = 'Add';
    } else if (message.includes('Updated')) {
      operation = 'Update';
    } else if (message.includes('Deleted')) {
      operation = 'Delete';
    }

    this.toastr.success(`Employee ${operation} successful`, 'Notification', {
      timeOut: 3000,
      progressBar: true,
      positionClass: 'toast-top-right',
    });
  }
  ngOnDestroy() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }
}